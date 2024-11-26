namespace SachkovTech.Issues.Contracts.Requests.Issue;

public record AddIssueRequest(
    Guid ModuleId,
    Guid? LessonId,
    string Title,
    string Description,
    int Experience);