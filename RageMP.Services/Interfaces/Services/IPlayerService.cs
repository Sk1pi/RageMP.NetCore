using RageMP.NetCore.Domain.Entities;
using RageMP.Services.DTOs;

namespace RageMP.Services.Interfaces.Services;

public interface IPlayerService
{
    Task<Player> CreatePlayerAsync(string username);
    
    Task UpdatePlayerProfileAsync(Guid userId, PlayerCreateDto profileData);
    
    Task<Player> GetPlayerByIdAsync(Guid userId);

    Task UpdateLastLogoutAsync(
        Guid userId,
        float x,
        float y,
        float z,
        float heading,
        int dimension);
}