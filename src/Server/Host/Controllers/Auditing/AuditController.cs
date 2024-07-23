using MultiMart.Application.Auditing.Interfaces;
using MultiMart.Application.Auditing.Models;
using MultiMart.Application.Auditing.Request.Queries;
using MultiMart.Application.Common.Models;

namespace MultiMart.Host.Controllers.Auditing;

public class AuditController : VersionNeutralApiController
{
    [HttpPost("search")]
    [RequiresPermission(ApplicationAction.Search, ApplicationResource.AuditLogs)]
    [SwaggerOperation("Search audit logs using available filters.", "")]
    public async Task<ActionResult<PaginationResponse<AuditDto>>> SearchAsync(SearchAuditRequest request)
        => await Mediator.Send(request);
}