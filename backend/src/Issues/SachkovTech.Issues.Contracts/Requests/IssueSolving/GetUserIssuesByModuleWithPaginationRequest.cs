namespace SachkovTech.Issues.Contracts.Requests.IssueSolving;

public record GetUserIssuesByModuleWithPaginationRequest(
    Guid UserId,
    Guid ModuleId,
    string? Status,
    int Page,
    int PageSize);