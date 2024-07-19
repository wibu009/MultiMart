using MultiMart.Application.Auditing.Interfaces;
using MultiMart.Application.Auditing.Models;
using MultiMart.Application.Common.Models;

namespace MultiMart.Application.Auditing.Request.Queries;

public class SearchAuditRequest : AuditListFilter, IRequest<PaginationResponse<AuditDto>>
{
}

public class SearchAuditRequestHandler : IRequestHandler<SearchAuditRequest, PaginationResponse<AuditDto>>
{
    private readonly IAuditService _auditService;

    public SearchAuditRequestHandler(IAuditService auditService) =>
        _auditService = auditService;

    public Task<PaginationResponse<AuditDto>> Handle(SearchAuditRequest request, CancellationToken cancellationToken) =>
        _auditService.SearchAsync(request);
}