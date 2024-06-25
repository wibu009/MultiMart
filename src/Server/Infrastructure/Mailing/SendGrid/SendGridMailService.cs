using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MultiMart.Application.Common.Mailing;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MultiMart.Infrastructure.Mailing.SendGrid;

public class SendGridMailService : ISendGridMailService
{
    private readonly SendGridMailSettings _sendGridMailSettings;
    private readonly ILogger<SendGridMailService> _logger;
    private readonly SendGridClient _client;

    public SendGridMailService(IOptions<SendGridMailSettings> sendGridSettings, ILogger<SendGridMailService> logger)
    {
        _logger = logger;
        _sendGridMailSettings = sendGridSettings.Value;
        _client = new SendGridClient(_sendGridMailSettings.ApiKey);
    }

    public async Task SendAsync(MailRequest request, CancellationToken ct)
    {
        // From
        var from = new EmailAddress(request.From ?? _sendGridMailSettings.From, _sendGridMailSettings.DisplayName);

        // To, Cc, Bcc
        var to = request.To.ConvertAll(email => new EmailAddress(email));
        var cc = request.Cc.ConvertAll(email => new EmailAddress(email));
        var bcc = request.Bcc.ConvertAll(email => new EmailAddress(email));

        // Only works if cc, bcc, to are less than 1000 because of SendGrid's limitation
        var personalization = new List<Personalization>
        {
            new()
            {
                Tos = to,
                Ccs = cc,
                Bccs = bcc
            }
        };

        // Subject
        string subject = request.Subject;

        // Body
        string? body = request.Body;

        // Headers
        var headers = request.Headers.ToDictionary(header => header.Key, header => header.Value);

        var msg = new SendGridMessage()
        {
            From = from,
            Subject = subject,
            PlainTextContent = body,
            HtmlContent = body,
            Personalizations = personalization,
            Headers = headers
        };

        // ReplyTo
        if (!string.IsNullOrEmpty(request.ReplyTo))
        {
            msg.ReplyTo = new EmailAddress(request.ReplyTo);
        }

        // Attachment
        if (request.AttachmentData.Any())
        {
            var attachments = request.AttachmentData.Select(attachment =>
                new Attachment
                {
                    Content = Convert.ToBase64String(attachment.Value),
                    Filename = attachment.Key,
                    Type = attachment.Key.Split('.').Last()
                }).ToList();

            msg.AddAttachments(attachments);
        }

        msg.SetClickTracking(false, false);

        var response = await _client.SendEmailAsync(msg, ct);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            _logger.LogError("Failed to send email. Status code: {StatusCode}, Body: {Body}", response.StatusCode,
                await response.Body.ReadAsStringAsync(ct));
            throw new Exception("Failed to send email");
        }
    }
}