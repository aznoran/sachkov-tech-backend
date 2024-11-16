namespace TagService.Features.Queries.GetTagById;

public record GetTagsWithPaginationQuery(
    Guid? Cursor,
    int Limit);