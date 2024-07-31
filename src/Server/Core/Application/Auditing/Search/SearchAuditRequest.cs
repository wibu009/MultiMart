namespace MultiMart.Application.Auditing.Search;

public class SearchAuditRequest : PaginationFilter, IRequest<PaginationResponse<AuditDto>>
{
}