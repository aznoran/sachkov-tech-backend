namespace FaqService.Extensions;

public class PaginatedList<T>
{
    public List<T> Items { get; init; }
    public int PageNumber { get; init;}
    public int PageSize { get; init;}
    public int TotalCount { get; init;}
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public PaginatedList(List<T> items, int pageNumber, int pageSize, int totalCount)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}