namespace FaqService.Features.Commands.Post.UpdateRefsAndTags;

public record UpdatePostRefAndTagsCommand(
    Guid Id,
    string ReplLink,
    Guid? IssueId,
    Guid? LessonId,
    List<Guid>? Tags);