namespace CommentService.Extensions;

public class CursorPagedList<T>
    // where T : Entity<Guid>
{
    public IReadOnlyList<T> Items { get; init; } = [];

    public long? TotalCount { get; init; }

    public int Limit { get; init; }

    public Guid? Cursor { get; init; }

    public Guid? NextCursor { get; init; }

    public bool HasNextPage => NextCursor.HasValue;

    public CursorPagedList(
        IReadOnlyList<T> items, 
        Guid? cursor, 
        Guid? nextCursor, 
        int limit, 
        long? totalCount = null)
    {
        Items = items;
        Cursor = cursor;
        Limit = limit;
        TotalCount = totalCount;
    }
}