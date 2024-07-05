using MultiMart.Application.Auditing.Dtos;
using MultiMart.Application.Common.Models;

namespace MultiMart.Application.Auditing.Request.Queries;

public class SearchAuditRequest : IRequest<PaginationResponse<AuditDto>>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; } = int.MaxValue;
    public Guid? UserId { get; set; }
    public string? Type { get; set; }
    public string? TableName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string[]? OrderBy { get; set; }
}