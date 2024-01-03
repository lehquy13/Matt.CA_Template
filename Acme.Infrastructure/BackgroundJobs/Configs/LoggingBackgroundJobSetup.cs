using Microsoft.Extensions.Options;
using Quartz;

namespace Acme.Infrastructure.BackgroundJobs.Configs;

internal class LoggingBackgroundJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = new JobKey(nameof(LoggingBackgroundJob));
        options
            .AddJob<LoggingBackgroundJob>(builder => builder.WithIdentity(jobKey))
            .AddTrigger(trigger => trigger.ForJob(jobKey)
                .WithIdentity(nameof(LoggingBackgroundJob))
                .WithCronSchedule(
                    "1 0/2 * * * ?")); // You can use https://www.freeformatter.com/cron-expression-generator-quartz.html to generate cron expression
    }
}