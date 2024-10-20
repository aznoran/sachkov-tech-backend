using SachkovTech.Core.Abstractions;

namespace SachkovTech.IssueSolving.Application.Commands.SendForRevision;

public record SendForRevisionCommand(
    Guid UserIssueId) : ICommand;