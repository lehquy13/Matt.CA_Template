using Acme.Application.Contracts.Interfaces.Infrastructures;
using Acme.Infrastructure.EmailServices;
using Matt.ResultObject;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.Extensions.Options;
using Quartz;

namespace Acme.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution] // Mark that this job can't be run in parallel
internal class PdfReportBackgroundJob(
        IAppLogger<PdfReportBackgroundJob> logger,
        IEmailSender emailSender,
        IOptions<EmailSettings> emailSettings)
    : IJob
{
    private async Task<Result> GeneratePdfAsync()
    {
        await Task.CompletedTask;
        try
        {
            var html =
                $"<h1>Your report at {DateTime.Now:dd-MM-yyyy}</h1>" +
                $"<h2>Reported by Matt system</h2>";

            var render = new ChromePdfRenderer();
            var pdf = render.RenderHtmlAsPdf(html);

            var root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var fileName = $"Matt report in {DateTime.Now:dd-MM-yyyy}.pdf";

            if (!Directory.Exists(root)) Directory.CreateDirectory(root);

            var fullPath = Path.Combine(root, fileName);

            pdf.SaveAs(fullPath);

            var file = PdfDocument.FromFile(Path.Combine(root, fileName));

            if (file is null) throw new Exception($"Fail to find file {fileName}");

            await emailSender.SendEmailWithAttachment(
                emailSettings.Value.ManagerEmail,
                $"Library report in {DateTime.Now:dd-MM-yyyy}",
                html,
                fullPath,
                true);

            return Result.Success();
        }
        catch (Exception e)
        {
            logger.LogError($"Generate pdf error: {e.Message}");
            return Result.Fail("Generate pdf error: {e.Message}");
        }
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var result = await GeneratePdfAsync();

        if (result.IsFailure)
        {
            logger.LogError($"Generate pdf error: {result.DisplayMessage}");
            return;
        }

        logger.LogInformation($"Generate pdf success at {DateTime.Now:dd-MM-yyyy hh:mm:ss}");
    }
}