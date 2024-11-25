namespace SachkovTech.Issues.Contracts.Requests.Lesson;

public record UpdateLessonRequest(
    Guid LessonId,
    string Title,
    string Description,
    int Experience,
    Guid VideoId,
    Guid PreviewId,
    IEnumerable<Guid> Tags,
    IEnumerable<Guid> Issues);
