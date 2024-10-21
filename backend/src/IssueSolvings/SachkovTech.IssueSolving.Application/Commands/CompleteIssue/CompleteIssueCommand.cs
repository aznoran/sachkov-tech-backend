using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssueSolving.Application.Commands.CompleteIssue;

public record CompleteIssueCommand(
    Guid UserIssueId) : ICommand;