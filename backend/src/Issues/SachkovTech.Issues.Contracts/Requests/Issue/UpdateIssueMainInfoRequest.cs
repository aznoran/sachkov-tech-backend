namespace SachkovTech.Issues.Contracts.Requests.Issue;

public record UpdateIssueMainInfoRequest(
    Guid LessonId, 
    Guid ModuleId,
    string Title,
    string Description,
    int Experience);