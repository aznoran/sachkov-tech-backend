using SachkovTech.Issues.Application.Features.Issue.Commands.AddIssue;

namespace SachkovTech.Issues.Presentation.Issues.Requests;

public record AddIssueRequest(
    Guid? LessonId,
    string Title,
    string Description,
    int Experience)
{
    public AddIssueCommand ToCommand(Guid moduleId) =>
        new(LessonId, moduleId, Title, Description, Experience);
}