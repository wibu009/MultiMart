namespace MultiMart.Application.Auditing.Search;

public class SearchAuditRequestHandler : IRequestHandler<SearchAuditRequest, PaginationResponse<AuditDto>>
{
    private readonly IAuditService _auditService;

    public SearchAuditRequestHandler(IAuditService auditService) =>
        _auditService = auditService;

    public Task<PaginationResponse<AuditDto>> Handle(SearchAuditRequest request, CancellationToken cancellationToken) =>
        _auditService.SearchAsync(request);
}