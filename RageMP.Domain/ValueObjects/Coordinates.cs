namespace RageMP.NetCore.Domain.ValueObjects;

public struct Coordinates
{
    public float X { get; }
    public float Y { get; }
    public float Z { get; }

    public Coordinates(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    public override string ToString()
    {
        return $"X:{X:F2}, Y:{Y:F2}, Z:{Z:F2}";
    }
}