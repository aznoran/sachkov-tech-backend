using SachkovTech.Issues.Application.Features.IssueSolving.Queries.GetUserIssuesByModuleWithPagination;

namespace SachkovTech.Issues.Presentation.IssueSolving.Requests;

public record GetUserIssuesByModuleWithPaginationRequest(
    Guid UserId,
    Guid ModuleId,
    string? Status,
    int Page,
    int PageSize)
{
    public GetUserIssuesByModuleWithPaginationQuery ToQuery() => 
        new (UserId, ModuleId, Status, Page, PageSize);
}