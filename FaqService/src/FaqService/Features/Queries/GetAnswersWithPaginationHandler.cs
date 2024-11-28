using FaqService.Dtos;
using FaqService.Extensions;
using FaqService.Infrastructure;

namespace FaqService.Features.Queries
{
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
            var answersQuery = _dbContext.Posts
                .Where(p => p.Id == postId)
                .SelectMany(p => p.Answers)
                .OrderByDescending(a => a.CreatedAt)
                .AsQueryable();

            var paginatedAnswers = await answersQuery.ToCursorList(query.Cursor, query.Limit, cancellationToken);

            var answerDtos = paginatedAnswers.Items.Select(a => new AnswerDto
            {
                Id = a.Id,
                IsSolution = a.IsSolution,
                PostId = a.PostId,
                Text = a.Text,
                UserId = a.UserId,
                Rating = a.Rating,
                CreatedAt = a.CreatedAt,
            }).ToList();

            return new CursorList<AnswerDto>(
                items: answerDtos,
                cursor: paginatedAnswers.Cursor,
                nextCursor: paginatedAnswers.NextCursor,
                limit: paginatedAnswers.Limit,
                totalCount: paginatedAnswers.TotalCount
            );
        }
    }
}