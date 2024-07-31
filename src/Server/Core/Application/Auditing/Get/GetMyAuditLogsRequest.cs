using MultiMart.Application.Common.Interfaces;

namespace MultiMart.Application.Auditing.Get;

public class GetMyAuditLogsRequest : IRequest<List<AuditDto>>
{
}