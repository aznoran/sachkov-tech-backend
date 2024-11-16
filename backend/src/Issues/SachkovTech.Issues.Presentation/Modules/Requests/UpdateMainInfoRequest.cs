using SachkovTech.Issues.Application.Features.Modules.Commands.UpdateMainInfo;

namespace SachkovTech.Issues.Presentation.Modules.Requests;

public record UpdateMainInfoRequest(
    string Title,
    string Description)
{
    public UpdateMainInfoCommand ToCommand(Guid moduleId) =>
        new(moduleId, Title, Description);
}