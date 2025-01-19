using Auth.Contracts.DTOs;
using Auth.Contracts.ExternalServices;
using Auth.Domain.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Auth.Infrastructure.ExternalApis;

public class WbsToolApiClient(
    HttpClient httpClient,
    IConfiguration configuration,
    IScopedLogger scopedLogger)
    : IWbsToolApiClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly IConfiguration _configuration = configuration;
    private readonly IScopedLogger _scopedLogger = scopedLogger;

    public async Task UpdatePersonAsync(PersonDto personDto, string jwt)
    {
        var url = $"{_configuration["WBStoolAPI:Url"]}/persons/{personDto.Id}";
        _scopedLogger.Log(url);
        _scopedLogger.Log(jwt);

        var request = new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = JsonContent.Create(personDto)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}
