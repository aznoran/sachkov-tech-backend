using CSharpFunctionalExtensions;
using TagService.Entities;
using TagService.HelperClasses;
using TagService.Infrastructure;

namespace TagService.Features.Commands.AddTag;

public class AddTagHandler
{
    private readonly ApplicationDbContext _context;

    public AddTagHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Tag, Error>> Handle(AddTagCommand command, CancellationToken cancellationToken)
    {
        var tag = Tag.Cretate(
            command.Name, 
            command.Description, 
            command.CreatedAt, 
            command.UsagesCount);

        if (tag.IsFailure)
            return tag.Error;
        
        await _context.Tags.AddAsync(tag.Value, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return tag.Value;
    }
}