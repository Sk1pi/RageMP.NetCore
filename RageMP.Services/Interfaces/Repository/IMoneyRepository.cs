using RageMP.Infrastructure.Interfaces.Repository;
using RageMP.NetCore.Domain.Entities;

namespace RageMP.Services.Interfaces.Repository;

public interface IMoneyRepository : IRepository<MoneyRecord>
{
    Task<decimal> GetCurrentBalanceAsync(Guid userId, CancellationToken cancellationToken = default);
}