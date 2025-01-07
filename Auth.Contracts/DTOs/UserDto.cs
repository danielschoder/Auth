namespace Auth.Contracts.DTOs;

public record UserDto(Guid Id, string Email, string Jwt);
