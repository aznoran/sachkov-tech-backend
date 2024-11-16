using Microsoft.EntityFrameworkCore;
using TagService.Entities;
using TagService.Infrastructure;

namespace TagService.Features.Queries.GetTagsByIdsList;

public class GetTagsByListIdsHandler
{
    private readonly ApplicationDbContext _dbContext;

    public GetTagsByListIdsHandler(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public async Task<List<Tag>> Handle(GetTagsByIdsListQuery query, CancellationToken cancellationToken)
    {
        var ids = query.Ids ?? [];

        if (!ids.Any())
        {
            return [];
        }

        var tagsQuery = _dbContext.Tags
            .Where(t => ids.Contains(t.Id));

        return await tagsQuery.ToListAsync(cancellationToken);
    }
}