using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Common.Mailing;

public interface IMailService : ITransientService
{
    Task SendAsync(MailRequest request, CancellationToken ct);
}