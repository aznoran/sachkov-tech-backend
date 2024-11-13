using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Dtos;
using SachkovTech.Core.Extensions;
using SachkovTech.Core.Models;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Application.Features.Issue.Queries.GetIssuesByModuleWithPagination;

public class GetIssuesByModuleWithPaginationHandler
    : IQueryHandlerWithResult<PagedList<IssueDto>, GetFilteredIssuesByModuleWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;

    public GetIssuesByModuleWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<Result<PagedList<IssueDto>, ErrorList>> Handle(
        GetFilteredIssuesByModuleWithPaginationQuery query,
        CancellationToken cancellationToken = default)
    {
        var moduleResult = await _readDbContext.Modules
            .SingleOrDefaultAsync(m => m.Id == query.ModuleId, cancellationToken);

        if (moduleResult is null)
            return Errors.General.NotFound(query.ModuleId).ToErrorList();

        var issuesQuery = _readDbContext.Issues
            .Where(m => m.ModuleId == query.ModuleId);

        Expression<Func<IssueDto, object>> keySelector = query.SortBy?.ToLower() switch
        {
            "title" => (issue) => issue.Title,
            _ => (issue) => issue.Id
        };

        issuesQuery = query.SortDirection?.ToLower() == "desc"
            ? issuesQuery.OrderByDescending(keySelector)
            : issuesQuery.OrderBy(keySelector);

        issuesQuery = issuesQuery.WhereIf(
            !string.IsNullOrWhiteSpace(query.Title),
            i => i.Title.Contains(query.Title!));

        return await issuesQuery
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}