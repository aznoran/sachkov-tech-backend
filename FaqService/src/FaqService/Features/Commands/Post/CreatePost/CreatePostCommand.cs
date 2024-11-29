namespace FaqService.Features.Commands.Post.CreatePost;

public record CreatePostCommand(
    string Title,
    string Description,
    string ReplLink,
    Guid UserId,
    Guid? IssueId,
    Guid? LessonId,
    List<Guid>? Tags);