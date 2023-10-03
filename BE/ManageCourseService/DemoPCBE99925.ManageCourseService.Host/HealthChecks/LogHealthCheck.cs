using Arc4u.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace EG.DemoPCBE99925.ManageCourseService.Host.HealthChecks;

public class LogHealthCheck : IHealthCheck
{

    public LogHealthCheck(ILogger<LogHealthCheck> logger)
    {
        _logger = logger;
    }

    private ILogger<LogHealthCheck> _logger;

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new HealthCheckResult(HealthStatus.Healthy, "No business check issue"));
    }
}