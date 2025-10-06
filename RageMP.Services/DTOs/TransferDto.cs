namespace RageMP.Services.DTOs;

public record TransferDto
{
    public Guid SourceUserId { get; set; }
    public Guid TargetUserId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }

    public TransferDto() { }
}