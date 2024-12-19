namespace Acme.Application.Interfaces;

public interface IEmailSender
{
    Task Send(string email, string subject, string message);
    Task SendHtml(string email, string subject, string template);
}