using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TagService.HelperClasses;
using TagService.Infrastructure;

namespace TagService.Features.Commands.UpdateTag.MainInfo;

public class UpdateMainInfoTagHandler
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateMainInfoTagHandler(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateMainInfoTagCommand command,
        CancellationToken cancellationToken)
    {
        var tag = await _dbContext.Tags
            .FirstOrDefaultAsync(t => t.Id == command.TagId, cancellationToken);
        
        if(tag is null)
            return Error.NotFound("Tag not found");
        
        var result = tag.Edit(command.Name, command.Description);
        if(result.IsFailure)
            return result.Error;
        
        _dbContext.Tags.Attach(tag);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return tag.Id;
    }
}