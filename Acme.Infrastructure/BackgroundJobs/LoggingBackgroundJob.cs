using Microsoft.Extensions.Logging;
using Quartz;

namespace Acme.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution] // Mark that this job can't be run in parallel
internal class LoggingBackgroundJob(ILogger<LoggingBackgroundJob> logger) : IJob
{
    private static int _count = 0;

    public async Task Execute(IJobExecutionContext context)
    {
        await Task.CompletedTask;
        logger.LogInformation("Checking background service working: count {Count} at {DateTime}", _count,
            DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
        _count++;
    }
}