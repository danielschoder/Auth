using Auth.Contracts.Requests;

namespace Auth.Contracts.Responses;

public record UserUpdateResponse(
    UserUpdateRequest UserUpdateRequest = null,
    string ErrorMessage = null);
