using FaqService.Dtos;
using FaqService.Entities;
using FaqService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FaqService.Features.Queries;

public class GetAnswerAtPostByIdHandler
{
    private readonly ApplicationDbContext _dbContext;

    public GetAnswerAtPostByIdHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AnswerDto?> Handle(
        Guid postId,  
        Guid answerId,  
        CancellationToken cancellationToken = default)
    {
        var answer = await _dbContext.Posts
            .Where(p => p.Id == postId) 
            .SelectMany(p => p.Answers) 
            .FirstOrDefaultAsync(a => a.Id == answerId, cancellationToken);
            
        return answer is not null
            ? new AnswerDto
            {
                Id = answer.Id,
                IsSolution = answer.IsSolution,
                PostId = answer.PostId,
                Text = answer.Text,
                UserId = answer.UserId,
                Rating = answer.Rating,
                CreatedAt = answer.CreatedAt,
            }
            : null;
    }
}