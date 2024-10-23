using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.Module.Commands.AddIssue;

public record AddIssueCommand(
    Guid ModuleId,
    string Title,
    string Description,
    int Experience) : ICommand;