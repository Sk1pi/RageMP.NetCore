using Microsoft.Extensions.Logging;
using RageMP.Infrastructure.Interfaces.Repository;
using RageMP.NetCore.Domain.Entities;
using RageMP.Services.DTOs;
using RageMP.Services.Interfaces.Services;

namespace RageMP.Services.Services;

public class PlayerService : IPlayerService
{
    private readonly IRepository<Player> _playerRepository;
    private readonly ILogger<PlayerService> _logger;

    public PlayerService(
        IRepository<Player> playerRepository,
        ILogger<PlayerService> logger)
    {
        _playerRepository = playerRepository;
        _logger = logger;
    }
    
    public async Task<Player> CreatePlayerAsync(string username)
    {
        var newPlayer = new Player
        {
            Id = Guid.NewGuid(),
            SocialClubName = username,
            CreatedAt = DateTime.UtcNow,
            LastLogin = DateTime.UtcNow 
        };
        
        await _playerRepository.AddAsync(newPlayer);
        await _playerRepository.SaveChangesAsync(); 

        _logger.LogInformation("New player created: {PlayerId} ({Username})", newPlayer.Id, username);

        return newPlayer;
    }

    public async Task UpdatePlayerProfileAsync(Guid userId, PlayerCreateDto profileData)
    {
        var player = await _playerRepository.GetByIdAsync(userId);

        if (player == null)
        {
            _logger.LogWarning("Attempted to update non-existent user: {UserId}", userId);
            throw new KeyNotFoundException($"Player with ID {userId} not found.");
        }
        
        player.SocialClubName = profileData.SocialClubName ?? player.SocialClubName; 

        _playerRepository.Update(player);
        await _playerRepository.SaveChangesAsync();

        _logger.LogInformation("Player profile updated for {UserId}", userId);
    }

    public async Task<Player> GetPlayerByIdAsync(Guid userId)
    {
        var player = await _playerRepository.GetByIdAsync(userId);
        
        if (player == null)
        {
            _logger.LogWarning("Requested non-existent user: {UserId}", userId);
            throw new KeyNotFoundException($"Player with ID {userId} not found.");
        }
        
        return player;
    }

    public async Task UpdateLastLogoutAsync(
        Guid userId, 
        float x, 
        float y, 
        float z, 
        float heading, 
        int dimension)
    {
        var player = await _playerRepository.GetByIdAsync(userId);
    
        if (player == null) return;

        // Оновлення полів сутності
        player.LastLogout = DateTime.UtcNow;
        player.PositionX = x;
        player.PositionY = y;
        player.PositionZ = z;
        player.Heading = heading;
        player.Dimension = dimension;

        _playerRepository.Update(player);
        await _playerRepository.SaveChangesAsync();
    }
}