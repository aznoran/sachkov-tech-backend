using FaqService.Dtos;
using FaqService.Extensions;
using FaqService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FaqService.Features.Queries;

public class GetAnswersWithCursorHandler
{
    private readonly ApplicationDbContext _dbContext;

    public GetAnswersWithCursorHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CursorList<AnswerDto>> Handle(
        Guid postId,
        Guid? cursor,
        int limit = 10,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Posts
            .Where(p => p.Id == postId)
            .SelectMany(p => p.Answers)
            .OrderByDescending(a => a.CreatedAt)
            .AsQueryable();

        if (cursor.HasValue)
        {
            query = query.Where(a => a.Id < cursor.Value);
        }
        
        var items = await query
            .Take(limit + 1) 
            .Select(a => new AnswerDto
            {
                Id = a.Id,
                IsSolution = a.IsSolution,
                PostId = a.PostId,
                Text = a.Text,
                UserId = a.UserId,
                Rating = a.Rating,
                CreatedAt = a.CreatedAt,
            })
            .ToListAsync(cancellationToken);
        
        Guid? nextCursor = null;
        if (items.Count > limit)
        {
            nextCursor = items.Last().Id;
            items = items.Take(limit).ToList();
        }

        return new CursorList<AnswerDto>(
            items: items,
            cursor: cursor,
            nextCursor: nextCursor,
            limit: limit);
    }
}