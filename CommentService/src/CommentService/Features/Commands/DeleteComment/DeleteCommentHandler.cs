using CommentService.HelperClasses;
using CommentService.Infrastructure;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Features.Commands.DeleteComment;

public class DeleteCommentHandler(ApplicationDbContext dbContext)
{
    public async Task<UnitResult<Error>> Handle(DeleteCommentCommand command, CancellationToken cancellationToken)
    {
        var comment = await dbContext.Comments
            .FirstOrDefaultAsync(c => c.Id == command.IdComment, cancellationToken);

        if (comment is null)
            return Error.NotFound("Comment not found");

        dbContext.Comments.Remove(comment);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success<Error>();
    }
}