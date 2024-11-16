namespace SachkovTech.Issues.Application.Features.Lessons;

public record LessonResponse(
    Guid Id,
    Guid ModuleId,
    string Title,
    string Description,
    int Experience,
    Guid VideoId,
    string VideoUrl,
    Guid PreviewId,
    string PreviewUrl,
    Guid[] Tags,
    Guid[] Issues);