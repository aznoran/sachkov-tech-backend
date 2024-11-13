using Microsoft.EntityFrameworkCore;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Dtos;
using SachkovTech.Core.Extensions;
using SachkovTech.Core.Models;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.SharedKernel.ValueObjects;

namespace SachkovTech.Issues.Application.Features.Modules.Queries.GetModulesWithPagination;

public record GetModulesWithPaginationQuery(string? Title, int PageNumber, int PageSize) : IQuery;

public class GetModulesWithPaginationHandler : IQueryHandler<PagedList<ModuleDto>, GetModulesWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;
    public GetModulesWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<ModuleDto>> Handle(GetModulesWithPaginationQuery query, CancellationToken cancellationToken = default)
    {
        var modulesQuery = _readDbContext.Modules.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Title))
        {
            modulesQuery = modulesQuery.Where(m => EF.Functions.Like(m.Title.ToLower(), $"%{query.Title.ToLower()}%"));
        }

        return await modulesQuery.ToPagedList(query.PageNumber, query.PageSize, cancellationToken);
    }
}