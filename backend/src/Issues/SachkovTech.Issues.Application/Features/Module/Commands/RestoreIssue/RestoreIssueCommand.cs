using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.Module.Commands.RestoreIssue;

public record RestoreIssueCommand(Guid ModuleId, Guid IssueId) : ICommand;