namespace Auth.Contracts.Requests;

public record UserUpdateRequest(string Email, string Name, string NickName);
