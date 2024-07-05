using Mapster;
using Microsoft.EntityFrameworkCore;
using MultiMart.Application.Auditing;
using MultiMart.Application.Auditing.Dtos;
using MultiMart.Application.Auditing.Interfaces;
using MultiMart.Application.Auditing.Request.Queries;
using MultiMart.Application.Common.Models;
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

    public async Task<PaginationResponse<AuditDto>> SearchAsync(SearchAuditRequest request)
    {
        var query = _context.AuditTrails.AsQueryable();

        if (request.UserId.HasValue)
            query = query.Where(a => a.UserId == request.UserId);

        if (!string.IsNullOrWhiteSpace(request.Type))
            query = query.Where(a => a.Type == request.Type);

        if (!string.IsNullOrWhiteSpace(request.TableName))
            query = query.Where(a => a.TableName == request.TableName);

        if (request.StartDate.HasValue)
            query = query.Where(a => a.DateTime >= request.StartDate);

        if (request.EndDate.HasValue)
            query = query.Where(a => a.DateTime <= request.EndDate);

        if(request.OrderBy?.Any() == true)
        {
            query = request.OrderBy.Aggregate(query, (current, orderBy) => orderBy switch
            {
                "DateTime" => current.OrderBy(a => a.DateTime),
                "DateTime desc" => current.OrderByDescending(a => a.DateTime),
                "Type" => current.OrderBy(a => a.Type),
                "Type desc" => current.OrderByDescending(a => a.Type),
                "TableName" => current.OrderBy(a => a.TableName),
                "TableName desc" => current.OrderByDescending(a => a.TableName),
                _ => current
            });
        }

        int totalRecords = await query.CountAsync();

        var trails = await query
            .OrderByDescending(a => a.DateTime)
            .Skip(request.PageSize * (request.PageNumber - 1))
            .Take(request.PageSize)
            .ToListAsync();

        return new PaginationResponse<AuditDto>(trails.Adapt<List<AuditDto>>(), totalRecords, request.PageNumber, request.PageSize);
    }
}