using Microsoft.Extensions.Logging;
using RageMP.Infrastructure.Interfaces.Repository;
using RageMP.NetCore.Domain.Entities;
using RageMP.Services.DTOs;
using RageMP.Services.Interfaces.DataBase;
using RageMP.Services.Interfaces.Repository;
using RageMP.Services.Interfaces.Services;

namespace RageMP.Services.Services;

public class MoneyService : IMoneyService
{
    private readonly IMoneyRepository _moneyRepository;
    private readonly ILogger<MoneyService> _logger;
    private readonly IRepository<ProcessedMessage> _processedMessageRepository;

    public MoneyService(
        IMoneyRepository moneyRepository,
        ILogger<MoneyService> logger, 
        IRepository<ProcessedMessage> processedMessageRepository)
    {
        _moneyRepository = moneyRepository;
        _logger = logger;
        _processedMessageRepository = processedMessageRepository;
    }
    
    public async Task<Guid> ExecuteTransactionAsync(TransferDto transactionData, Guid idempotencyKey)
    {
        var existingMessage = await _processedMessageRepository.GetByIdAsync(idempotencyKey);
        if (existingMessage != null)
        {
            _logger.LogWarning("Transaction with idempotency key {Key} already processed.", idempotencyKey);
            return idempotencyKey; 
        }
        
        var moneyRecord = new MoneyRecord
        {
            Id = Guid.NewGuid(),
            UserId = transactionData.SourceUserId,
            Amount = transactionData.Amount,
            Description = transactionData.Description
        };
        
        var processedMessage = new ProcessedMessage
        {
            Id = idempotencyKey,
            MessageType = nameof(ExecuteTransactionAsync)
        };
        
        await _moneyRepository.AddAsync(moneyRecord);
        await _processedMessageRepository.AddAsync(processedMessage); 
        
        await _moneyRepository.SaveChangesAsync(); 
        
        _logger.LogInformation("Transaction {TransactionId} executed successfully with idempotency key {Key}", moneyRecord.Id, idempotencyKey);

        return moneyRecord.Id;
    }

    public async Task<decimal> GetCurrentBalanceAsync(Guid userId)
    {
        return await _moneyRepository.GetCurrentBalanceAsync(userId); 
    }
}