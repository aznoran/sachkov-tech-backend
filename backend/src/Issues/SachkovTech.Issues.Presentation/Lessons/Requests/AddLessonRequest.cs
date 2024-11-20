using FileService.Contracts;
using SachkovTech.Issues.Application.Features.Lessons.Command.AddLesson;

namespace SachkovTech.Issues.Presentation.Lessons.Requests;

public record AddLessonRequest(
    Guid ModuleId,
    string Title,
    string Description,
    int Experience,
    Guid VideoId,
    Guid PreviewId,
    IEnumerable<Guid> Tags,
    IEnumerable<Guid> Issues,
    string FileName,
    string ContentType,
    long FileSize,
    string UploadId,
    List<PartETagInfo> Parts)
{
    public AddLessonCommand ToCommand() =>
        new(ModuleId,
            Title,
            Description,
            Experience,
            VideoId,
            PreviewId,
            Tags,
            Issues,
            FileName,
            ContentType,
            FileSize,
            UploadId,
            Parts);
}