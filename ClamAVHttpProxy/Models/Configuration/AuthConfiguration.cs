namespace ClamAVHttpProxy.Models.Configuration;

public class AuthConfiguration
{
    public string[] ValidApiKeys { get; init; } = Array.Empty<string>();
}