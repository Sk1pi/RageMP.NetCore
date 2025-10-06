using System.ComponentModel.DataAnnotations;

namespace RageMP.NetCore.Domain.Entities;

public class MoneyRecord
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    [StringLength(250)]
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Completed";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}