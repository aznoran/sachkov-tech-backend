using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace TagService.Extensions;

public static class CursorPaginationQueriesExtensions
{
    public static async Task<CursorList<T>> ToCursorList<T>(
        this IQueryable<T> source,
        Guid? cursor,
        int limit,
        CancellationToken cancellationToken = default)
        where T : Entity<Guid>
    {
        var totalCount = await source.CountAsync(cancellationToken);

        var items = await source
            .Where(x => x.Id > cursor || !cursor.HasValue)
            .Take(limit)
            .ToListAsync(cancellationToken);

        Guid? nextCursor = items.Any() ? items.Last().Id : null;

        return new CursorList<T>(
            items,
            cursor,
            nextCursor,
            limit,
            totalCount);
    }

    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> source,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }
}