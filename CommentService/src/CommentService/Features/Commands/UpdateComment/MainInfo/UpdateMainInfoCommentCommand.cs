namespace CommentService.Features.Commands.UpdateComment.MainInfo;

public record UpdateMainInfoCommentCommand(
    Guid IdComment,
    string Text);