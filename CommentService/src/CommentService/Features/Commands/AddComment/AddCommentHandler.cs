using CommentService.Entities;
using CommentService.HelperClasses;
using CommentService.Infrastructure;
using CSharpFunctionalExtensions;

namespace CommentService.Features.Commands.AddComment;

public class AddCommentHandler(ApplicationDbContext dbContext)
{
    public async Task<Result<Guid, Error>> Handle(AddCommentCommand command, CancellationToken cancellationToken)
    {
        var commentResult = Comment.Create(
            command.RelationId,
            command.UserId,
            command.RepliedId,
            command.Text,
            command.Rating);
        
        if (commentResult.IsFailure)
            return commentResult.Error;

        await dbContext.Comments.AddAsync(commentResult.Value, cancellationToken);
        
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return commentResult.Value.Id;
    }
}