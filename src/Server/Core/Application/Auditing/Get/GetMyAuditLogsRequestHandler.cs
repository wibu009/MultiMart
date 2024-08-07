﻿using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Auditing.Get;

public class GetMyAuditLogsRequestHandler : IRequestHandler<GetMyAuditLogsRequest, List<AuditDto>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IAuditService _auditService;

    public GetMyAuditLogsRequestHandler(ICurrentUser currentUser, IAuditService auditService) =>
        (_currentUser, _auditService) = (currentUser, auditService);

    public Task<List<AuditDto>> Handle(GetMyAuditLogsRequest request, CancellationToken cancellationToken) =>
        _auditService.GetUserTrailsAsync(_currentUser.GetUserId());
}