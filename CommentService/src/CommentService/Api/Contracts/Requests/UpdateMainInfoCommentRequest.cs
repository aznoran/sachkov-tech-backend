using CommentService.Features.Commands.UpdateComment.MainInfo;

namespace CommentService.Api.Contracts.Requests;

public record UpdateMainInfoCommentRequest(string Text)
{
    public UpdateMainInfoCommentCommand ToCommand(Guid idComment) => new(idComment, Text);
}