using Microsoft.EntityFrameworkCore;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Models;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Contracts.Module;

namespace SachkovTech.Issues.Application.Features.Modules.Queries.GetModulesWithPagination;

public record GetModulesWithPaginationQuery(string? Title, int Page, int PageSize) : IQuery;

public class GetModulesWithPaginationHandler : IQueryHandler<PagedList<ModuleResponse>, GetModulesWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;
    public GetModulesWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<ModuleResponse>> Handle(GetModulesWithPaginationQuery query, CancellationToken cancellationToken = default)
    {
        var modulesQuery = _readDbContext.Modules.AsQueryable();
        
        var totalCount = await modulesQuery.CountAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(query.Title))
        {
            modulesQuery = modulesQuery.Where(m => EF.Functions.Like(m.Title.ToLower(), $"%{query.Title.ToLower()}%"));
        }

        var modules = modulesQuery.ToList()
            .Select(i => new ModuleResponse
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    IssuesPosition = i.IssuesPosition,
                }
            );

        return new PagedList<ModuleResponse>
        {
            Items = modules.Skip((query.Page-1) * query.PageSize).ToList(),
            TotalCount = totalCount,
            PageSize = query.PageSize,
            Page = query.Page
        };
    }
}