namespace SachkovTech.Core.Dtos;

public class ModuleDto
{
    public Guid Id { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public IssuePositionDto[] IssuesPosition { get; init; } = [];

    public bool IsDeleted { get; init; }
}

public class IssuePositionDto
{
    public Guid IssueId { get; init; }

    public int Position { get; init; } = default!;
}

public class LessonPositionDto
{
    public Guid LessonId { get; init; }

    public int Position { get; init; } = default!;
}