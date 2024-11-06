using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.Module.Commands.Delete;

public record DeleteModuleCommand(Guid ModuleId) : ICommand;