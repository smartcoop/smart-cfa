namespace Core.SeedWork;

public class PaginatedList<T> : List<T> {

    public int PageIndex  { get; }
    public int PageSize   { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }

    public PaginatedList(IQueryable<T> source, int pageIndex, int pageSize) {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = source.Count();
        TotalPages = (int) Math.Ceiling(TotalCount / (double)PageSize);

        this.AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
    }

    public bool HasPreviousPage {
        get {
            return (PageIndex > 0);
        }
    }

    public bool HasNextPage {
        get {
            return (PageIndex+1 < TotalPages);
        }
    }
}


public class PagedList<T> : List<T> {

    public int PageIndex  { get; }
    public int PageSize   { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }

    public PagedList(IEnumerable<T> source, PageItem pageItem, int count) {
        PageIndex = pageItem.PageIndex;
        PageSize = pageItem.PageSize;
        TotalCount = count;
        TotalPages = (int) Math.Ceiling(TotalCount / (double)PageSize);
        this.AddRange(source);
    }

    public bool HasPreviousPage {
        get {
            return (PageIndex > 0);
        }
    }

    public bool HasNextPage {
        get {
            return (PageIndex+1 < TotalPages);
        }
    }
}

public class PageItem
{
    private int _currentPage;
    private int _pageSize;

    public int PageIndex
    {
        get => CurrentPage - 1;
    }
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            Guard.Requires(() => value > 0, "Current page cannot be 0");
            _currentPage = value;
        }
    }

    public int PageSize
    {
        get => _pageSize;
        set
        {
            Guard.Requires(() => value > 0, "Page size cannot be equal or under 0");
            _pageSize = value;
        }
    }

    public int Offset => PageIndex * PageSize;

    public PageItem(int currentPage, int pageSize)
    {
        CurrentPage = currentPage;
        PageSize = pageSize;
    }

}
