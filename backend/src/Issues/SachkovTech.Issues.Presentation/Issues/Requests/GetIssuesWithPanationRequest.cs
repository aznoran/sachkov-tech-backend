using SachkovTech.Issues.Application.Features.Issue.Queries.GetIssuesWithPagination;

namespace SachkovTech.Issues.Presentation.Issues.Requests;

public record GetIssuesWithPaginationRequest(
    string? Title,
    int? PositionFrom,
    int? PositionTo,
    string? SortBy,
    string? SortDirection,
    int Page,
    int PageSize)
{
    public GetFilteredIssuesWithPaginationQuery ToQuery() =>
        new(Title, PositionFrom, PositionTo, SortBy, SortDirection, Page, PageSize);
}