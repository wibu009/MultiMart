using Ardalis.Specification.EntityFrameworkCore;
using Mapster;
using Microsoft.EntityFrameworkCore;
using MultiMart.Application.Auditing.Interfaces;
using MultiMart.Application.Auditing.Models;
using MultiMart.Application.Common.Models;
using MultiMart.Application.Common.Specification;
using MultiMart.Infrastructure.Persistence.Context;

namespace MultiMart.Infrastructure.Auditing;

public class AuditService : IAuditService
{
    private readonly ApplicationDbContext _context;

    public AuditService(ApplicationDbContext context) => _context = context;

    public async Task<List<AuditDto>> GetUserTrailsAsync(Guid userId)
    {
        var trails = await _context.AuditTrails
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.DateTime)
            .Take(250)
            .ToListAsync();

        return trails.Adapt<List<AuditDto>>();
    }

    public async Task<PaginationResponse<AuditDto>> SearchAsync(AuditListFilter filter)
    {
        var pagingSpec = new EntitiesByPaginationFilterSpec<Trail>(filter);
        var trails = await _context.AuditTrails
            .WithSpecification(pagingSpec)
            .ProjectToType<AuditDto>()
            .ToListAsync();

        var filterSpec = new EntitiesByBaseFilterSpec<Trail>(filter);
        int count = await _context.AuditTrails
            .WithSpecification(filterSpec)
            .CountAsync();

        return new PaginationResponse<AuditDto>(trails, count, filter.PageNumber, filter.PageSize);
    }
}