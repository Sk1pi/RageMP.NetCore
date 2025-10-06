namespace RageMP.NetCore.Domain.ValueObjects;

public struct Coordinates
{
    private float X { get; }
    private float Y { get; }
    private float Z { get; }

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