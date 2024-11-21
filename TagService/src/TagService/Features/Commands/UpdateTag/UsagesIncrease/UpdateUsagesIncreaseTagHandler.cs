using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TagService.HelperClasses;
using TagService.Infrastructure;

namespace TagService.Features.Commands.UpdateTag.UsagesIncrease;

public class UpdateUsagesIncreaseTagHandler
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateUsagesIncreaseTagHandler(ApplicationDbContext context)
    {
        _dbContext = context;
    }
    
    public async Task<Result<Guid, Error>> Handle(UpdateUsagesIncreaseTagCommand command, CancellationToken cancellationToken)
    {
        var tag = await _dbContext.Tags
            .FirstOrDefaultAsync(t => t.Id == command.TagId, cancellationToken);
        
        if (tag is null)
            return Error.NotFound("Comment not found");
        
        tag.UsagesIncrease();

        _dbContext.Tags.Attach(tag);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return tag.Id;
    }
}