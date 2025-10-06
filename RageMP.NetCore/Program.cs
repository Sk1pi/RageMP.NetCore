using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RageMP.Infrastructure.Data;
using RageMP.Infrastructure.Interfaces.Repository;
using RageMP.Infrastructure.Interfaces.Repository.ReadOnly;
using RageMP.Infrastructure.Repositories;
using RageMP.NetCore;
using RageMP.NetCore.Domain.Entities;
using RageMP.Services.DTOs;
using RageMP.Services.Extensions;
using RageMP.Services.Interfaces.DataBase;
using RageMP.Services.Interfaces.Repository;
using RageMP.Services.Interfaces.Services;
using RageMP.Services.Services;

public class Program : Rage.Events.Script 
{
    private readonly ILogger<Program> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    
    const string PostgreSQLConnectionString = "Host=localhost;Database=RageMP_DB;Username=user;Password=12345678"; 
    
    public Program(ILogger<Program> logger)
    {
        _logger = logger;
        var services = new ServiceCollection();
        
        ConfigureServices(services);
        
        var serviceProvider = services.BuildServiceProvider();
        _scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        
        InitializeDatabase(serviceProvider);
        
        RegisterRageEvents();
        
        _logger.LogInformation("[RageMP .NET] Server script fully initialized.");
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IApplicationDbContext, ApplicationDbContext>();
        
        services.AddLogging(configure => configure.AddConsole());

        services.AddTransient(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>));
        services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

        services.AddTransient<IMoneyRepository, MoneyRepository>();

        services.AddTransient<IPlayerService, PlayerService>();
        services.AddTransient<IMoneyService, MoneyService>();
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(PostgreSQLConnectionString); 
        }, ServiceLifetime.Singleton); 
    }
    
    private void InitializeDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        try
        {
            dbContext.Database.Migrate(); 
            _logger.LogInformation("[Infrastructure] Database initialized and migrations applied.");
        }
        catch (Exception ex)
        {
            throw new Exception($"[FATAL] DB Migration failed: {ex.Message}");
        }
    }
    
    private void RegisterRageEvents()
    {
        Rage.Events.Add(Rage.Events.Type.PlayerJoin, OnPlayerJoin);
        Rage.Events.Add(Rage.Events.Type.PlayerQuit, OnPlayerQuit);
        
        Rage.Events.Add(Rage.Events.Type.PlayerCommand, OnPlayerCommand);
    }
    
    private async void OnPlayerJoin(Player player)
    {
        using var scope = _scopeFactory.CreateScope();
        var playerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation("Player {Name} joined. ID: {Id}", player.SocialClubName, player.Id);
            
            var dbPlayer = await playerService.CreatePlayerAsync(player.SocialClubName);
            
            player.Position = new Vector3(-103.3f, 497.5f, 169.1f);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing PlayerJoin for {Name}", player.SocialClubName);
        }
    }
    
    private async void OnPlayerQuit(Player player, Rage.Events.DisconnectReason type, string message)
    {
        using var scope = _scopeFactory.CreateScope();
        var playerService = scope.ServiceProvider.GetRequiredService<IPlayerService>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation($"Player {player.SocialClubName} disconnected. Reason: {player.Reason}", player.SocialClubName, player.Reason);
            
            var userId = player.GetPlayerGuid(); 
        
            await playerService.UpdateLastLogoutAsync(
                userId, 
                player.Position.X, 
                player.Position.Y, 
                player.Position.Z,
                player.Heading,   
                player.Dimension  
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing PlayerQuit for {Name} (ID: {Id})", player.SocialClubName, player.Id);
        }
    }

    private async void OnPlayerCommand(Player player, string text)
{
    using var scope = _scopeFactory.CreateScope();
    var serviceProvider = scope.ServiceProvider;
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        var parts = text.Split(' ');
        var command = parts[0].ToLower();
        var userId = player.GetPlayerGuid();

        switch (command)
        {
            case "/pos":
                player.OutputChatBox($"[POS]: X={player.Position.X:F2}, Y={player.Position.Y:F2}, Z={player.Position.Z:F2}, Heading={player.Heading:F2}, Dimension={player.Dimension}");
                break;

            case "/money":
                var moneyQueryService = serviceProvider.GetRequiredService<IMoneyService>();
                var balance = await moneyQueryService.GetCurrentBalanceAsync(userId); 
                player.OutputChatBox($"[BANK]: Your current balance: ${balance:N2}");
                break;
                
            case "/give":
                if (parts.Length < 3 || !int.TryParse(parts[2], out var amount) || amount <= 0)
                {
                    player.OutputChatBox("[ERROR]: Usage: /give [player] [amount]\");\n");
                    return;
                }
                
                var targetPlayer = Rage.Entities.Players.All.FirstOrDefault(p => p.SocialClubName.Equals(parts[1], StringComparison.OrdinalIgnoreCase));

                if (targetPlayer == null)
                {
                    player.OutputChatBox("[ERROR]: Player not found.");
                    return;
                }
                
                var transferData = new TransferDto
                {
                    SourceUserId = userId,
                    TargetUserId = targetPlayer.GetPlayerGuid(),
                    Amount = amount
                };
                
                var moneyCommandService = serviceProvider.GetRequiredService<IMoneyService>();
                await moneyCommandService.ExecuteTransactionAsync( 
                    transferData, 
                    Guid.NewGuid()); 

                player.OutputChatBox($"[BANK]: You have successfully transferred {amount} to the player {targetPlayer}.");
                targetPlayer.OutputChatBox($"[BANK]: You received ${amount} from {player.SocialClubName}.");
                break;

            case "/tp":
                if (parts.Length < 4 || !float.TryParse(parts[1], out var x) || !float.TryParse(parts[2], out var y) || !float.TryParse(parts[3], out var z))
                {
                    player.OutputChatBox("[ERROR]: Using: /tp [x] [y] [z]");
                    return;
                }
                player.Position = new Vector3(x, y, z);
                player.OutputChatBox($"[TP]: \nTeleported to {x:F2}, {y:F2}, {z:F2}");
                break;
                
            case "/veh": 
                if (parts.Length < 2)
                {
                    player.OutputChatBox("[ERROR]: Usage: /veh [vehicle_model]");
                    return;
                }
    
                var modelName = parts[1];
    
                try
                {
                    var vehicleHash = Rage.Hash.Get(modelName);
                    
                    Rage.Entities.Vehicles.Create(
                        vehicleHash, 
                        player.Position, 
                        player.Heading, 
                        player.Dimension
                    );
                    
                    player.OutputChatBox($"[VEH]: Transport successfully spawned '{modelName}'.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to spawn vehicle {Model} for player {Name}.", modelName, player.SocialClubName);
                    player.OutputChatBox($"[ERROR]: Failed to spawn vehicle '{modelName}'. Please check the model name.");
                }
                break;

            default:
                player.OutputChatBox($"[ERROR]: \nUnknown team {command}.");
                break;
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error processing command '{Text}' from player {Name}", text, player.SocialClubName);
        player.OutputChatBox("[ERROR]: An error occurred on the server.");
    }
}
}