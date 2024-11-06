using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.Module.Commands.UpdateMainInfo;

public record UpdateMainInfoCommand(Guid ModuleId, string Title, string Description) : ICommand;