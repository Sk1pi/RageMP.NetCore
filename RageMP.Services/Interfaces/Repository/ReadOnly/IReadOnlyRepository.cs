using System.Linq.Expressions;
using RageMP.Infrastructure.Interfaces.Specification;

namespace RageMP.Infrastructure.Interfaces.Repository.ReadOnly;

public interface IReadOnlyRepository<T> where T : class
{
    Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken = default);

    Task<T?> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    
    Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
    
    Task<List<TResult>> ListAsync<TResult>(
        ISpecification<T> specification,
        Expression<Func<T, TResult>> selector,
        CancellationToken cancellationToken = default) where TResult : class;

    Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);
}