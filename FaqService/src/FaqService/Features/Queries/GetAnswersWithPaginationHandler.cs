using FaqService.Dtos;
using FaqService.Entities;
using FaqService.Extensions;
using FaqService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FaqService.Features.Queries;

public class GetAnswersWithPaginationHandler
{
    private readonly ApplicationDbContext _dbContext;

    public GetAnswersWithPaginationHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PaginatedList<AnswerDto>> Handle(
        Guid postId,  
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Posts
            .Where(p => p.Id == postId)
            .SelectMany(p => p.Answers)
            .OrderByDescending(a => a.CreatedAt);
                
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
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

        return new PaginatedList<AnswerDto>(items, pageNumber, pageSize, totalCount);
    }
}