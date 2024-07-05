using MultiMart.Application.Auditing.Dtos;
using MultiMart.Application.Auditing.Interfaces;
using MultiMart.Application.Auditing.Request.Queries;
using MultiMart.Application.Common.Models;
using MultiMart.Shared.Authorization;

namespace MultiMart.Host.Controllers.Auditing;

public class AuditController : VersionNeutralApiController
{
    private readonly IAuditService _auditService;

    public AuditController(IAuditService auditService) =>
        _auditService = auditService;

    [HttpPost("search")]
    [RequiresPermission(ApplicationAction.Search, ApplicationResource.AuditLogs)]
    [SwaggerOperation("Search audit logs using available filters.", "")]
    public async Task<ActionResult<PaginationResponse<AuditDto>>> SearchAsync(SearchAuditRequest request) =>
        await _auditService.SearchAsync(request);
}