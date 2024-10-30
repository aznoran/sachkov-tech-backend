namespace CommentService.Features.Commands.AddComment;

public record AddCommentCommand(
    Guid RelationId,
    Guid UserId,
    Guid RepliedId,
    string Text,
    int Rating);