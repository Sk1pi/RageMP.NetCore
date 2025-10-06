using System.Linq.Expressions;

namespace RageMP.Infrastructure.Interfaces.Specification;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    
    List<Expression<Func<T, object>>> Includes { get; }
    
    Expression<Func<T, object>> OrderBy { get; }
    bool OrderByDescending { get; }
    
    int Skip { get; }
    int Take { get; }
    bool IsPagingEnabled { get; }
}