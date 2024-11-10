using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.Issue.Commands.UpdateIssueMainInfo;

public record UpdateIssueMainInfoCommand(
    Guid IssueId,
    string Title,
    string Description,
    int Experience) : ICommand;