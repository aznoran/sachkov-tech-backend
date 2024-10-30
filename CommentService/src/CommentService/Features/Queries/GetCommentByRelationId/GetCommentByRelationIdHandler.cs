using CommentService.Entities;
using CommentService.Extensions;
using CommentService.Infrastructure;

namespace CommentService.Features.Queries.GetCommentByRelationId;

public class GetCommentByRelationIdWithPaginationHandler(ApplicationDbContext dbContext)
{
    public async Task<CursorPagedList<Comment>> Handle(
        GetCommentByRelationIdWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var commentsQuery = dbContext.Comments.AsQueryable();

        commentsQuery = query.SortDirection?.ToLower() == "desc"
            ? commentsQuery.OrderByDescending(c => c.Id)
            : commentsQuery.OrderBy(c => c.Id);

        commentsQuery = commentsQuery.Where(x => x.RelationId == query.RelationId);

        return await commentsQuery.ToPagedList(query.Cursor, query.Limit, cancellationToken);
    }
}