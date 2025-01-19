using Auth.Contracts.DTOs;

namespace Auth.Contracts.ExternalServices;

public interface IWbsToolApiClient
{
    Task UpdatePersonAsync(PersonDto personDto, string jwt);
}
