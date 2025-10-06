namespace RageMP.Services.DTOs;

public record PlayerCreateDto(
    long Id,
    string SocialClubName,
    decimal MoneyBalance);