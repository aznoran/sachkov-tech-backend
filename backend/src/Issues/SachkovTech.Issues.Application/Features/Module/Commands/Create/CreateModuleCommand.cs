using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.Module.Commands.Create;

public record CreateModuleCommand(string Title, string Description) : ICommand;