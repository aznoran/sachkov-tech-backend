using SachkovTech.Issues.Application.Features.Issue.Commands.UpdateIssueMainInfo;

namespace SachkovTech.Issues.Presentation.Modules.Requests;

public record UpdateIssueMainInfoRequest(
    string Title,
    string Description,
    int Experience)
{
    public UpdateIssueMainInfoCommand ToCommand(Guid issueId) =>
        new(issueId, Title, Description, Experience);
}