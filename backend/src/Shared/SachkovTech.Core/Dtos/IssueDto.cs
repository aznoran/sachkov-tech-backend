namespace SachkovTech.Core.Dtos;

public class IssueDto
{
    public Guid Id { get; init; }
    
    public bool IsDeleted { get; init; }
    
    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public int Position { get; init; }
    
    public Guid? LessonId { get; init; }
    
    public Guid? ModuleId { get; init; }
    
    public Guid[] Files { get; init; } = [];
}