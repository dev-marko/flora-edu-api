using Microsoft.EntityFrameworkCore;

namespace FloraEdu.Domain.DataTransferObjects;

public class PagedList<T>
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 10;

    public List<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasNextPage => Page * PageSize < TotalCount;
    public bool HasPreviousPage => Page > 1;

    private PagedList(List<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize)
    {
        page = page == 0 ? DefaultPage : page;
        pageSize = pageSize == 0 ? DefaultPageSize : pageSize;

        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedList<T>(items, page, pageSize, totalCount);
    }
}
