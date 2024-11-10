using SachkovTech.Core.Abstractions;

namespace SachkovTech.Issues.Application.Features.Issue.Commands.AddIssue;

public record AddIssueCommand(
    string Title,
    string Description,
    int Experience) : ICommand;