using Nest;

namespace FaqService.Features.Elastic;

public class PostElastic
{
    [Keyword(Name = "id")]
    public Guid Id { get; init; }
    
    [Text(Name = "title")]
    public string Title { get; init; } = string.Empty;
    
    [Text(Name = "description")]
    public string Description { get; init; } = string.Empty;
    
    [Text(Name = "replLink")]
    public string ReplLink { get; init; } = string.Empty;
    
    [Keyword(Name = "status")]
    public string Status { get; init; } = string.Empty;
    
    [Date(Name = "createdAt")]
    public DateTime CreatedAt { get; init; }
    
    [Keyword(Name = "tags")]
    public List<Guid> Tags { get; init; } = [];

    [Keyword(Name = "issueId")]
    public Guid? IssueId { get; init; }

    [Keyword(Name = "lessonId")]
    public Guid? LessonId { get; init; }
    
}