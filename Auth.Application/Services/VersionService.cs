using System.Reflection;

namespace Auth.Application.Services;

public class VersionService : IVersionService
{
    public string GetVersion()
        => Assembly.GetEntryAssembly().GetName().Version.ToString();
}
