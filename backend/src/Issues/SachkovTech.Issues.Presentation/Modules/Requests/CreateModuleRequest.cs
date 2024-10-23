using SachkovTech.Issues.Application.Features.Module.Commands.Create;

namespace SachkovTech.Issues.Presentation.Modules.Requests;

public record CreateModuleRequest(string Title, string Description)
{
    public CreateModuleCommand ToCommand() => new (Title, Description);
}