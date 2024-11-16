using FaqService.Features.Commands.Post.UpdateRefsAndTags;

namespace FaqService.Api.Contracts;

public record UpdatePostRefAndTagsRequest(
    string ReplLink,
    Guid? IssueId,
    Guid? LessonId,
    List<Guid>? Tags)
{
    public UpdatePostRefAndTagsCommand ToCommand(Guid Id) => new(Id, ReplLink, IssueId, LessonId, Tags);
}