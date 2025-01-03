namespace Acme.Application.Contracts.Interfaces.Infrastructures;

public interface IEmailSender
{
    Task SendEmail(string email, string subject, string message, bool isHtml = false);
    Task SendEmailWithAttachment(string email, string subject, string message, string fullPath, bool isHtml = false);
}