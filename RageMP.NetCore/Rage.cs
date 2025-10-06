using System.Numerics;
using RageMP.NetCore.Domain.Entities;

namespace RageMP.NetCore;

public static class Rage 
{
    public static class Events 
    {
        public enum Type { PlayerJoin, PlayerQuit, PlayerCommand } 
        
        public enum DisconnectReason { Unknown, Timeout, Kick, Ban } 
        
        public static void Add(Type eventType, Delegate handler) {}

        public class Script { }
    }
    
    public static class Entities 
    {
        public static class Players
        {
            public static List<Player> All { get; } = new List<Player>(); 
        }
        
        public static class Vehicles
        {
            public static Vehicle Create(ulong model, Vector3 position, float heading, int dimension)
            {
                Console.WriteLine($"[VEHICLE STUB]: Created vehicle {model} at {position.X}.");
                return new Vehicle(); 
            }
        }
    }

    public static class Hash 
    {
        public static ulong Get(string modelName)
        {
            return (ulong)modelName.GetHashCode();
        }
    }
}
