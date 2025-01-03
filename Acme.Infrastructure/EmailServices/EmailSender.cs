using System.Net;
using System.Net.Mail;
using Acme.Application.Contracts.Interfaces.Infrastructures;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.Extensions.Options;

namespace Acme.Infrastructure.EmailServices;

internal class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IOptions<EmailSettings> options)
    {
        _emailSettings = options.Value;
    }

    public async Task SendEmail(string email, string subject, string message, bool isHtml = false)
    {
        var senderMail = _emailSettings.Email;
        var pw = _emailSettings.Password;

        var client = new SmtpClient(_emailSettings.SmtpClient, _emailSettings.Port)
        {
            EnableSsl = _emailSettings.EnableSsl
        };
        client.UseDefaultCredentials = _emailSettings.UseDefaultCredentials;
        client.Credentials = new NetworkCredential(senderMail, pw);

        Email.DefaultSender = new SmtpSender(client);

        //should make use of this
        var response = await Email
            .From(senderMail).To(email)
            .Subject(subject).Body(message, isHtml)
            .SendAsync();
    }

    public async Task SendEmailWithAttachment(string email, string subject, string message, string fullPath,
        bool isHtml = false)
    {
        var senderMail = _emailSettings.Email;
        var pw = _emailSettings.Password;

        var client = new SmtpClient(_emailSettings.SmtpClient, _emailSettings.Port)
        {
            EnableSsl = _emailSettings.EnableSsl
        };
        client.UseDefaultCredentials = _emailSettings.UseDefaultCredentials;
        client.Credentials = new NetworkCredential(senderMail, pw);

        Email.DefaultSender = new SmtpSender(client);

        //should make use of this
        var response = await Email
            .From(senderMail).To(email)
            .Subject(subject).Body(message, isHtml)
            .AttachFromFilename(fullPath,
                attachmentName: Path.GetFileName(fullPath))
            .SendAsync();
    }
}