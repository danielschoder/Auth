using Auth.Contracts.DTOs;

namespace Auth.Contracts.Responses;

public record AuthResponse(UserDto User = null, string ErrorMessage = null, bool Authorized = true);
