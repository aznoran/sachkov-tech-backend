using SachkovTech.Issues.Application.Features.Module.Commands.UpdateIssueMainInfo;

namespace SachkovTech.Issues.Presentation.Modules.Requests;

public record UpdateIssueMainInfoRequest(
    string Title,
    string Description,
    int Experience)
{
    public UpdateIssueMainInfoCommand ToCommand(Guid moduleId, Guid issueId) =>
        new(moduleId, issueId, Title, Description, Experience);
}