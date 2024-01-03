using Microsoft.Extensions.Options;
using Quartz;

namespace Acme.Infrastructure.BackgroundJobs.Configs;

internal class ReportBackgroundJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = new JobKey(nameof(PdfReportBackgroundJob));
        options
            .AddJob<PdfReportBackgroundJob>(builder => builder.WithIdentity(jobKey))
            .AddTrigger(trigger => trigger.ForJob(jobKey)
                .WithIdentity(nameof(PdfReportBackgroundJob))
                .WithCronSchedule(
                    "0 59 23 * * ?")); // You can use https://www.freeformatter.com/cron-expression-generator-quartz.html to generate cron expression
        //  .WithCronSchedule("0/30 * * * * ?"));
    }
}