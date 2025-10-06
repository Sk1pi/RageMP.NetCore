using Microsoft.EntityFrameworkCore;
using RageMP.NetCore.Domain.Entities;
using RageMP.Services.Interfaces.DataBase;

namespace RageMP.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<ProcessedMessage> ProcessedMessages { get; set; }
    public DbSet<MoneyRecord> MoneyRecords { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ProcessedMessage>().HasKey(x => x.Id);
    }
}

