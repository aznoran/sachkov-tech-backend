namespace SachkovTech.Issues.Contracts.Dtos;

public class IssuePositionDto
{
    public Guid IssueId { get; init; }

    public int Position { get; init; } = default!;
}