using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Dtos;
using SachkovTech.Core.Extensions;
using SachkovTech.Core.Models;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Domain.IssueSolving.Enums;

namespace SachkovTech.Issues.Application.Features.IssueSolving.Queries.GetUserIssuesByModuleWithPagination;

public class GetUserIssuesByModuleWithPaginationHandler
    : IQueryHandler<PagedList<UserIssueDto>, GetUserIssuesByModuleWithPaginationQuery>
{
    private readonly IReadDbContext _readDbContext;
    
    public GetUserIssuesByModuleWithPaginationHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PagedList<UserIssueDto>> Handle(
        GetUserIssuesByModuleWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var userIssuesQuery = 
            from userIssue in _readDbContext.UserIssues 
            join issue in _readDbContext.Issues
                on userIssue.IssueId equals issue.Id
            where userIssue.UserId == query.UserId 
                  && userIssue.ModuleId == query.ModuleId 
                  && userIssue.Status == query.Status
            orderby Enum.Parse<IssueStatus>(userIssue.Status)
            select new UserIssueDto()
            {
                Id = userIssue.Id,
                UserId = userIssue.UserId,
                IssueId = userIssue.IssueId,
                ModuleId = userIssue.ModuleId,
                IssueTitle = issue.Title,
                IssueDescription = issue.Description,
                Status = userIssue.Status,
                StartDateOfExecution = userIssue.StartDateOfExecution,
                EndDateOfExecution = userIssue.EndDateOfExecution,
                Attempts = userIssue.Attempts,
                PullRequestUrl = userIssue.PullRequestUrl
            };
        
        return await userIssuesQuery
            .ToPagedList(query.Page, query.PageSize, cancellationToken);
    }
}