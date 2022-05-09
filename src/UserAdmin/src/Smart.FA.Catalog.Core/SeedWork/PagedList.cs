namespace Smart.FA.Catalog.Core.SeedWork;

/// <summary>
/// The recommended Pagination class for EF-MVC of Microsoft (https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/sort-filter-page?view=aspnetcore-6.0)
/// is incompatible with complex mapping in LINQ-TO-SQL scenario.
/// For the specific use cases in the application, using the IQueryable interface is not possible for projecting tracked entities onto DTO models (used for displays only).
/// The PagedList and the PagedObject (here-below) classes are the middle-ground that I found between performance and usability. PagedObject contains information for
/// skipping and fetching a specific number of records according to the page while the PagedList keeps track of the total count of objects and pages. It uses IEnumerable
/// instead of IQueriable (so dynamic LINQ-TO-SQL generation is not possible, which is actually good to maintain good decoupling of responsibilities)
/// </summary>
public class PagedList<T> : List<T>
{
    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }

    public PagedList(IEnumerable<T> source, PageItem pageItem, int count)
    {
        PageIndex = pageItem.PageIndex;
        PageSize = pageItem.PageSize;
        TotalCount = count;
        TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
        this.AddRange(source);
    }

    public bool HasPreviousPage
    {
        get { return (PageIndex > 0); }
    }

    public bool HasNextPage
    {
        get { return (PageIndex + 1 < TotalPages); }
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
