using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Common.Mailing;

public interface IMailService : IScopedService
{
    Task SendAsync(MailRequest request, CancellationToken ct);
}