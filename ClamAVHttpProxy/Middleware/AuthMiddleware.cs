namespace ClamAVHttpProxy.Middleware;

using Microsoft.Extensions.Options;
using Models.Configuration;

public class AuthMiddleware
{
    private const string API_KEY_HEADER = "X-API-Key";
    private readonly ILogger<AuthMiddleware> _logger;
    private readonly RequestDelegate _next;
    private readonly HashSet<string> _validApiKeys;

    public AuthMiddleware(
        ILogger<AuthMiddleware> logger,
        RequestDelegate next,
        IOptions<AuthConfiguration> config)
    {
        _logger = logger;
        _next = next;
        _validApiKeys = new HashSet<string>(config.Value.ValidApiKeys);
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        var endpoint = ctx.GetEndpoint();

        if (endpoint?.DisplayName != "Health checks")
        {
            if (!ctx.Request.Headers.TryGetValue(API_KEY_HEADER, out var apiKey)
                || !_validApiKeys.Contains(apiKey)) {

                _logger.LogWarning("Unauthorised request from {IPAddress}.", ctx.Connection.RemoteIpAddress);
                ctx.Response.StatusCode = 401;
                return;
            }
        }

        _logger.LogDebug("Request authorised via API key.");

        await _next(ctx);
    }
}