using MultiMart.Application.Auditing.Models;
using MultiMart.Application.Auditing.Request.Queries;
using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Auditing.Interfaces;

public interface IAuditService : ITransientService
{
    Task<List<AuditDto>> GetUserTrailsAsync(DefaultIdType userId);
    Task<PaginationResponse<AuditDto>> SearchAsync(AuditListFilter filter);
}