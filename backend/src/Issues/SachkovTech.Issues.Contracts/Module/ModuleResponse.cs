using SachkovTech.Issues.Contracts.Dtos;

namespace SachkovTech.Issues.Contracts.Module;

public class ModuleResponse
{
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public IssuePositionDto[] IssuesPosition { get; init; } = [];
}