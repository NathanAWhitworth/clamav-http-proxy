namespace ClamAVHttpProxy.HealthChecks;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using nClam;

internal class ClamAVHealthCheck : IHealthCheck
{
    private readonly ClamClient _clamav;

    public ClamAVHealthCheck(ClamClient clamav)
    {
        _clamav = clamav;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext ctx, CancellationToken cancellationToken = default)
    {
        return await _clamav.TryPingAsync(cancellationToken)
            ? HealthCheckResult.Healthy()
            : HealthCheckResult.Unhealthy("ClamAV unreachable.");
    }
}