using MultiMart.Application.Auditing.Search;
using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Auditing;

public interface IAuditService : ITransientService
{
    Task<List<AuditDto>> GetUserTrailsAsync(DefaultIdType userId);
    Task<PaginationResponse<AuditDto>> SearchAsync(SearchAuditRequest filter);
}