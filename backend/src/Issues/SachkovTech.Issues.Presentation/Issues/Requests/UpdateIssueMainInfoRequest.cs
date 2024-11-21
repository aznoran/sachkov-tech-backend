using SachkovTech.Issues.Application.Features.Issue.Commands.UpdateIssueMainInfo;

namespace SachkovTech.Issues.Presentation.Issues.Requests;

public record UpdateIssueMainInfoRequest(
    Guid LessonId, 
    Guid ModuleId,
    string Title,
    string Description,
    int Experience)
{
    public UpdateIssueMainInfoCommand ToCommand(Guid issueId) =>
        new(issueId, LessonId, ModuleId, Title, Description, Experience);
}