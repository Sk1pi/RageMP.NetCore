using Microsoft.EntityFrameworkCore;
using RageMP.Infrastructure.Interfaces.Repository;
using RageMP.Infrastructure.Interfaces.Repository.ReadOnly;
using RageMP.Services.Interfaces.DataBase;

namespace RageMP.Services.Interfaces.Repository;

public class Repository<T> : ReadOnlyRepository<T>, IRepository<T> where T : class
{
    private readonly IApplicationDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public Repository(IApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<T>();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}