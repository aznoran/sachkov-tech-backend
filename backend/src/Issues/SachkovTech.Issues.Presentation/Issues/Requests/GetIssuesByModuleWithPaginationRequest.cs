using SachkovTech.Issues.Application.Features.Issue.Queries.GetIssuesByModuleWithPagination;

namespace SachkovTech.Issues.Presentation.Issues.Requests;

public record GetIssuesByModuleWithPaginationRequest(
    string? Title,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetFilteredIssuesByModuleWithPaginationQuery ToQuery(Guid moduleId) =>
        new(moduleId, Title, SortBy, SortDirection, Page, PageSize);
}