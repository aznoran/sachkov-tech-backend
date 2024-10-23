using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.Module.Commands.UpdateIssuePosition;

public record UpdateIssuePositionCommand(Guid ModuleId, Guid IssueId, int NewPosition) : ICommand;