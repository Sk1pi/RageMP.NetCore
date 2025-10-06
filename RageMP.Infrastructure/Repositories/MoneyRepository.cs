using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RageMP.Infrastructure.Data;
using RageMP.Infrastructure.Interfaces;
using RageMP.Infrastructure.Interfaces.Repository;
using RageMP.Infrastructure.Interfaces.Specification;
using RageMP.NetCore.Domain.Entities;
using RageMP.Services.Interfaces.Repository;

namespace RageMP.Infrastructure.Repositories;

public class MoneyRepository : Repository<MoneyRecord>, IMoneyRepository
{
    private readonly ApplicationDbContext _dbContext;
    public MoneyRepository(ApplicationDbContext dbContext) : base(dbContext) => _dbContext = dbContext;

    public async Task<decimal> GetCurrentBalanceAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.MoneyRecords
            .Where(m => m.UserId == userId)
            .SumAsync(r => r.Amount, cancellationToken);
    }
}