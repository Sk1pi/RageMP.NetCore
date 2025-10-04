namespace RageMP.NetCore.Domain.Entities;

public class Player
{
    public long Id { get; init; } 
    public string SocialClubName { get; init; } = string.Empty;
    public decimal MoneyBalance { get; init; }
    public Player() { }
    public Player(long id, string socialClubName, decimal initialBalance = 1000)
    {
        Id = id;
        SocialClubName = socialClubName;
        MoneyBalance = initialBalance;
    }
}