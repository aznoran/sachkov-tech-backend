using CommentService.Features.Commands.AddComment;

namespace CommentService.Api.Contracts.Requests;

public record AddCommentRequest(Guid RelationId, Guid UserId, Guid RepliedId, string Text, int Rating)
{
    public AddCommentCommand ToCommand() =>
        new(RelationId, UserId, RepliedId, Text, Rating);
};