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
        GetAnswerQuery query,
        CancellationToken cancellationToken = default)
    {
        var answers = _dbContext.Posts
            .Where(p => p.Id == postId)
            .SelectMany(p => p.Answers)
            .OrderByDescending(a => a.CreatedAt)
            .AsQueryable();

        if (query.Cursor.HasValue)
        {
            answers = answers.Where(a => a.Id < query.Cursor.Value);
        }
        
        var items = await answers
            .Take(query.Limit + 1) 
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
        if (items.Count > query.Limit)
        {
            nextCursor = items.Last().Id;
            items = items.Take(query.Limit).ToList();
        }

        return new CursorList<AnswerDto>(
            items: items,
            cursor: query.Cursor,
            nextCursor: nextCursor,
            limit: query.Limit);
    }
}