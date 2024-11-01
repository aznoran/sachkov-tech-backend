namespace CommentService.Features.Queries.GetCommentByRelationId;

public record GetCommentByRelationIdWithPaginationQuery(
    Guid RelationId,
    Guid? Cursor,
    int Limit);
