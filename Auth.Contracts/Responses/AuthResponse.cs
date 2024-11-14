namespace Auth.Contracts.Responses;

public record AuthResponse(string Jwt, string ErrorMessage, bool Unauthorized)
{
    public AuthResponse(string jwt)
        : this(jwt, null, false) { }
    public AuthResponse(string errorMessage, bool unauthorized)
        : this(null, errorMessage, unauthorized) { }
}
