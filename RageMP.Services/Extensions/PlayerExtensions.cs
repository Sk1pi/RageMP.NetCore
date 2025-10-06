using RageMP.NetCore.Domain.Entities;

namespace RageMP.Services.Extensions;

public static class PlayerExtensions
{
    public static Guid GetPlayerGuid(this Player player)
    {
        if (player.HasData("dbId") && player.GetData<Guid>("dbId") != Guid.Empty)
        {
            return player.GetData<Guid>("dbId");
        }
        
        throw new InvalidOperationException($"Player {player.SocialClubName} has no valid database ID attached.");
    }
}