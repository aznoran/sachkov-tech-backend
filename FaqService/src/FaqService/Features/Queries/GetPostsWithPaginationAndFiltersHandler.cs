using FaqService.Dtos;
using FaqService.Entities;
using FaqService.Enums;
using FaqService.Extensions;
using FaqService.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FaqService.Features.Queries
{
    public class GetPostsWithPaginationAndFiltersHandler
    {
        private readonly ApplicationDbContext _dbContext;

        public GetPostsWithPaginationAndFiltersHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedList<PostDto>> Handle(
            int pageNumber,
            int pageSize,
            string? searchText,
            Status? status,
            bool? sortByDateDescending,
            List<Guid>? tags,
            Guid? issueId,
            Guid? lessonId,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Posts.AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(p => p.GinIndex
                              .Matches(EF.Functions.PhraseToTsQuery("russian", searchText))
                              || EF.Functions.ILike(p.TrgmIndex, $"%{searchText}%"));
            }

            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status.Value);
            }

            if (tags != null && tags.Count != 0)
            {
                query = query.Where(p => p.Tags != null && p.Tags.Intersect(tags).Any());
            }
            
            if (issueId.HasValue)
            {
                query = query.Where(p => p.IssueId == issueId);
            }

            
            if (lessonId.HasValue)
            {
                query = query.Where(p => p.LessonId == lessonId);
            }

            query = (sortByDateDescending ?? true)
                ? query.OrderByDescending(p => p.CreatedAt)
                : query.OrderBy(p => p.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    ReplLink = p.ReplLink,
                    UserId = p.UserId,
                    IssueId = p.IssueId,
                    LessonId = p.LessonId,
                    Tags = p.Tags,
                    Status = p.Status,
                    AnswerId = p.AnswerId,
                    CreatedAt = p.CreatedAt,
                    CountOfAnswers = p.Answers.Count,
                })
                .ToListAsync(cancellationToken);


            return new PaginatedList<PostDto>(items, pageNumber, pageSize, totalCount);
        }
    }
}