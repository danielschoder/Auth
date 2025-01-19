using Auth.Contracts.DTOs;

namespace Auth.Contracts.ExternalServices;

public interface IWbsToolApiClient
{
    Task<bool> UpdatePersonAsync(PersonDto personDto, string jwt);
}
