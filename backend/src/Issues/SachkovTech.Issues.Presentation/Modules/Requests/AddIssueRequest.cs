using SachkovTech.Issues.Application.Features.Issue.Commands.AddIssue;

namespace SachkovTech.Issues.Presentation.Modules.Requests;

public record AddIssueRequest(
    string Title,
    string Description,
    int Experience)
{
    public AddIssueCommand ToCommand() =>
        new (Title, Description, Experience);
}