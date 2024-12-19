using Acme.Infrastructure.EmailServices;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Emails;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace Acme.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution] // Mark that this job can't be run in parallel
internal class PdfReportBackgroundJob(
    ILogger<PdfReportBackgroundJob> logger,
    IEmailSender emailSender,
    IOptions<EmailSettings> emailSettings
) : IJob
{
    private Task<Result> GeneratePdfAsync()
    {
        return Task.FromResult(Result.Success());
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var result = await GeneratePdfAsync();

        if (result.IsFailed)
        {
            logger.LogError("Generate pdf error: {ResultDisplayMessage}", result.DisplayMessage);
            return;
        }

        logger.LogInformation("Generate pdf success at {Now}", DateTimeProvider.Now);
    }
}