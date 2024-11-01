using CommentService.HelperClasses;
using CommentService.Infrastructure;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Features.Commands.UpdateComment.MainInfo;

public class UpdateMainInfoCommentHandler(ApplicationDbContext dbContext)
{
    public async Task<Result<Guid, Error>> Handle(UpdateMainInfoCommentCommand command, CancellationToken cancellationToken)
    {
        var comment = await dbContext.Comments
            .FirstOrDefaultAsync(c => c.Id == command.IdComment, cancellationToken);

        if (comment is null)
            return Error.NotFound("Comment not found");
        
        var result = comment.Edit(command.Text);
        if (result.IsFailure)
            return result.Error;
        
        dbContext.Comments.Attach(comment);
        await dbContext.SaveChangesAsync(cancellationToken);

        return comment.Id;
    }
}