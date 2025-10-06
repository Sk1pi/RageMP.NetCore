namespace RageMP.Services.DTOs;

public record MoneyRecordDto(
    Guid Id,
    Guid UserId,
    decimal Amount,
    string Description,
    string Status,
    DateTime CreatedAt);