using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TagService.HelperClasses;
using TagService.Infrastructure;

namespace TagService.Features.Commands.DeleteTag;

public class DeleteTagHandler
{
    private readonly ApplicationDbContext _dbContext;

    public DeleteTagHandler(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public async Task<UnitResult<Error>> Handle(DeleteTagCommand command, CancellationToken cancellationToken)
    {
        var tag = await _dbContext.Tags
            .FirstOrDefaultAsync(t => t.Id == command.TagId, cancellationToken);
        
        if(tag is null)
            return Error.NotFound("Tag not found");
        
        _dbContext.Tags.Remove(tag);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success<Error>();
    }
}