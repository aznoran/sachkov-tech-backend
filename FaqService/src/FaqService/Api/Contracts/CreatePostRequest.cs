using FaqService.Features.Commands.Post.CreatePost;

namespace FaqService.Api.Contracts;

public record CreatePostRequest(
    string Title,
    string Description,
    string ReplLink,
    Guid UserId,
    Guid? IssueId,
    Guid? LessonId,
    List<Guid>? Tags)
{
    public CreatePostCommand ToCommand() =>
        new(Title, Description, ReplLink, UserId, IssueId, LessonId, Tags);
}