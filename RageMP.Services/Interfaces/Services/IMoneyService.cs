using RageMP.Services.DTOs;

namespace RageMP.Services.Interfaces.Services;

public interface IMoneyService : ICommandService
{
    Task<Guid> ExecuteTransactionAsync(TransferDto transactionData, Guid idempotencyKey);
    Task<decimal> GetCurrentBalanceAsync(Guid userId);
}