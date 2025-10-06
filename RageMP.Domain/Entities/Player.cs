using System.Numerics;

namespace RageMP.NetCore.Domain.Entities;

public class Player
{
    public Guid Id { get; init; } 
    public string SocialClubName { get; set; } = string.Empty;
    public decimal MoneyBalance { get; init; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLogin { get; set; }
    public Vector3 Position { get; set; }
    public DateTime LastLogout { get; set; }
    public float PositionX { get; set; }
    public float PositionY { get; set; }
    public float PositionZ { get; set; }
    public float Heading { get; set; }
    public int Dimension { get; set; }
    public string Reason { get; set; }
    
    public void OutputChatBox(string message)
    {
        Console.WriteLine($"[CHAT SIMULATOR - {SocialClubName}]: {message}");
    }
    
    private readonly Dictionary<string, object> _dataStorage = new Dictionary<string, object>();
    
    public bool HasData(string key)
    {
        return _dataStorage.ContainsKey(key);
    }
    
    public T GetData<T>(string key)
    {
        if (_dataStorage.TryGetValue(key, out var value) && value is T typedValue)
        {
            return typedValue;
        }
        return default(T); 
    }
    public Player() { }
    public Player(Guid id, string socialClubName, decimal initialBalance = 1000)
    {
        Id = id;
        SocialClubName = socialClubName;
        MoneyBalance = initialBalance;
    }
}