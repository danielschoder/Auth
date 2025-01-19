namespace Auth.Contracts.DTOs;

public record PersonDto(
    Guid Id,
    string Email,
    string Name,
    string NickName);
