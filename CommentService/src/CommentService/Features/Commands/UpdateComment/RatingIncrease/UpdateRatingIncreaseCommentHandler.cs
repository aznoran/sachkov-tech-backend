using CommentService.HelperClasses;
using CommentService.Infrastructure;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Features.Commands.UpdateComment.RatingIncrease;

public class UpdateRatingIncreaseCommentHandler(ApplicationDbContext dbContext)
{
    public async Task<Result<Guid, Error>> Handle(UpdateRatingIncreaseCommentCommand command, CancellationToken cancellationToken)
    {
        var comment = await dbContext.Comments
            .FirstOrDefaultAsync(c => c.Id == command.IdComment, cancellationToken);
        
        if (comment is null)
            return Error.NotFound("Comment not found");
        
        comment.RatingIncrease();

        dbContext.Comments.Attach(comment);
        await dbContext.SaveChangesAsync(cancellationToken);

        return comment.Id;
    }
}