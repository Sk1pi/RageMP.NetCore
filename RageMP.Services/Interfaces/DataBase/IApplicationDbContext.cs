using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RageMP.NetCore.Domain.Entities;

namespace RageMP.Services.Interfaces.DataBase;

public interface IApplicationDbContext 
{
    DbSet<Player> Players { get; set; }
    DbSet<Vehicle> Vehicles { get; set; }
    DbSet<ProcessedMessage> ProcessedMessages { get; set; }
    DbSet<MoneyRecord> MoneyRecords { get; set; }
    
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}