namespace Auth.Contracts.Responses;

public record AuthResponse(string Jwt = null, string ErrorMessage = null, bool Unauthorized = false);
