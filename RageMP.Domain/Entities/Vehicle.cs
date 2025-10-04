using RageMP.NetCore.Domain.ValueObjects;

namespace RageMP.NetCore.Domain.Entities;

public class Vehicle
{
    public int Id { get; init; } 
    public uint ModelHash { get; init; } 
    public int OwnerId { get; init; } 
    public Coordinates Position { get; init; }
}