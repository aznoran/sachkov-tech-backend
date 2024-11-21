namespace TagService.Extensions;

public class CursorList<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];

    public long? TotalCount { get; init; }

    public int Limit { get; init; }

    public Guid? Cursor { get; init; }

    public Guid? NextCursor { get; init; }

    public bool HasNextCursor => NextCursor.HasValue;

    public CursorList(
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
        NextCursor = nextCursor;
    }
}