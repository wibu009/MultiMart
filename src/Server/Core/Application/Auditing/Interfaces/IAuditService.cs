using MultiMart.Application.Auditing.Dtos;
using MultiMart.Application.Auditing.Request.Queries;
using MultiMart.Application.Common.Interfaces;
using MultiMart.Application.Common.Models;

namespace MultiMart.Application.Auditing.Interfaces;

public interface IAuditService : ITransientService
{
    Task<List<AuditDto>> GetUserTrailsAsync(Guid userId);
    Task<PaginationResponse<AuditDto>> SearchAsync(SearchAuditRequest request);
}