using SachkovTech.Issues.Application.Features.Issue.Commands.AddIssue;

namespace SachkovTech.Issues.Presentation.Issues.Requests;

public record AddIssueRequest(
    string Title,
    string Description,
    int Experience)
{
    public AddIssueCommand ToCommand(Guid lessonId, Guid moduleId) =>
        new (lessonId, moduleId, Title, Description, Experience);
}