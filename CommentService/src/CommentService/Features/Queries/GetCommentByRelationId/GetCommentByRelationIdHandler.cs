using CommentService.Entities;
using CommentService.Extensions;
using CommentService.Infrastructure;

namespace CommentService.Features.Queries.GetCommentByRelationId;

public class GetCommentByRelationIdWithPaginationHandler(ApplicationDbContext dbContext)
{
    public async Task<CursorList<Comment>> Handle(
        GetCommentByRelationIdWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var commentsQuery = dbContext.Comments.AsQueryable();

        commentsQuery = commentsQuery.OrderBy(c => c.CreatedAt);

        commentsQuery = commentsQuery.Where(x => x.RelationId == query.RelationId);

        return await commentsQuery.ToCursorList(query.Cursor, query.Limit, cancellationToken);
    }
}