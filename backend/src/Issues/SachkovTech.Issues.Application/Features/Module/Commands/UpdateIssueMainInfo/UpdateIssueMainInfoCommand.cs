using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.Module.Commands.UpdateIssueMainInfo;

public record UpdateIssueMainInfoCommand(
    Guid ModuleId,
    Guid IssueId,
    string Title,
    string Description,
    int Experience) : ICommand;