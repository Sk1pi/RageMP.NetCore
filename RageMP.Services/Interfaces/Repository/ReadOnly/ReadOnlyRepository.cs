using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RageMP.Infrastructure.Interfaces.Specification;
using RageMP.Services.Interfaces.DataBase;

namespace RageMP.Infrastructure.Interfaces.Repository.ReadOnly;

public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class
{
    private readonly IApplicationDbContext _dbContext;
    private readonly DbSet<T> _dbSet;
    
    public ReadOnlyRepository(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    protected IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        return SpecificationEvaluator.GetQuery(_dbSet.AsQueryable(), specification);
    }
    
    public async Task<T?> GetByIdAsync(
        object id, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new[] { id }, cancellationToken);
    }

    public async Task<T?> GetBySpecAsync(
        ISpecification<T> specification, 
        CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<T>> ListAsync(
        ISpecification<T> specification, 
        CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).ToListAsync(cancellationToken);
    }

    public async Task<List<TResult>> ListAsync<TResult>(
        ISpecification<T> specification, 
        Expression<Func<T, TResult>> selector,
        CancellationToken cancellationToken = default) where TResult : class
    {
        var query = ApplySpecification(specification);
        
        return await query.Select(selector).ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(specification).CountAsync(cancellationToken);
    }
}