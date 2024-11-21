using FaqService.Enums;

namespace FaqService.Features.Queries;

public record GetPostsQuery(
    string? SearchText,
    Status? Status,
    bool? SortByDateDescending,
    List<Guid>? Tags,
    Guid? IssueId,
    Guid? LessonId,
    Guid? Cursor,
    int Limit = 10);