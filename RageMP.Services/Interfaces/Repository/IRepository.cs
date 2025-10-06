using RageMP.Infrastructure.Interfaces.Repository.ReadOnly;

namespace RageMP.Infrastructure.Interfaces.Repository;

public interface IRepository<T> : IReadOnlyRepository<T> where T : class
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    
    void Update(T entity);
    
    void Delete(T entity);
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}