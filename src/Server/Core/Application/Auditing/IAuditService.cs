using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Auditing;

public interface IAuditService : ITransientService
{
    Task<List<AuditDto>> GetUserTrailsAsync(Guid userId);
}