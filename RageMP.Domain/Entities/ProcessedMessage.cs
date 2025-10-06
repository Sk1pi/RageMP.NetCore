using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RageMP.NetCore.Domain.Entities;

[Table("ProcessedMessages")]
public class ProcessedMessage
{
    [Key]
    public Guid Id { get; init; }
    
    public DateTime ProcessedAt { get; init; } = DateTime.UtcNow;
    
    [StringLength(256)]
    public string? MessageType { get; init; }
}