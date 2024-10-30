namespace CommentService.Features.Queries.GetCommentByRelationId;

public record GetCommentByRelationIdWithPaginationQuery(
    Guid RelationId,
    Guid? Cursor,
    string? SortDirection,
    int Limit);
