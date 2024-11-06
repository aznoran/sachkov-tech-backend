using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.Module.Commands.DeleteIssue;

public record DeleteIssueCommand(Guid ModuleId, Guid IssueId) : ICommand;
