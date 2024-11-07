using TagService.Entities;
using TagService.Extensions;
using TagService.Infrastructure;

namespace TagService.Features.Queries.GetTagById;

public class GetTagByIdHandler
{
    private readonly ApplicationDbContext _dbContext;

    public GetTagByIdHandler(ApplicationDbContext context)
    {
        _dbContext = context;
    }
    
    public async Task<CursorList<Tag>> Handle(
        GetTagsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var tagsQuery = _dbContext.Tags.AsQueryable();

        tagsQuery = tagsQuery.OrderBy(t => t.CreatedAt);

        if (query.Cursor.HasValue)
        {
            tagsQuery = tagsQuery.Where(t => t.Id > query.Cursor.Value);
        }

        return await tagsQuery.ToCursorList(query.Cursor, query.Limit, cancellationToken);
    }
}