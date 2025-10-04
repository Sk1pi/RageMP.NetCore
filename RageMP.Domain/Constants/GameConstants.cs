using RageMP.NetCore.Domain.ValueObjects;

namespace RageMP.NetCore.Domain.Constants;

public class GameConstants
{
    public static readonly Coordinates DefaultSpawnPosition = new Coordinates(-103.7915f, -239.9922f, 44.91266f);
    public const decimal InitialMoney = 1000m;
    public const uint DefaultVehicleHash = 0x867384AE; 
    public const string DatabaseFileName = "RageMP.Infrastructure";
}