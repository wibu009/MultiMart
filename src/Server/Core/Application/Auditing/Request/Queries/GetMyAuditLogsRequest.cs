using MultiMart.Application.Auditing.Dtos;
using MultiMart.Application.Auditing.Interfaces;
using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Auditing.Request.Queries;

public class GetMyAuditLogsRequest : IRequest<List<AuditDto>>
{
}

public class GetMyAuditLogsRequestHandler : IRequestHandler<GetMyAuditLogsRequest, List<AuditDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IAuditService _auditService;

    public GetMyAuditLogsRequestHandler(ICurrentUser currentUser, IAuditService auditService) =>
        (_currentUser, _auditService) = (currentUser, auditService);

    public Task<List<AuditDto>> Handle(GetMyAuditLogsRequest request, CancellationToken cancellationToken) =>
        _auditService.GetUserTrailsAsync(_currentUser.GetUserId());
}