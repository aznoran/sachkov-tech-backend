using SachkovTech.Issues.Application.Features.Issue.Commands.AddIssue;

namespace SachkovTech.Issues.Presentation.Issues.Requests;

public record AddIssueRequest(
    Guid? ModuleId,
    Guid? LessonId,
    string Title,
    string Description,
    int Experience)
{
    public AddIssueCommand ToCommand() =>
        new(LessonId, ModuleId, Title, Description, Experience);
}