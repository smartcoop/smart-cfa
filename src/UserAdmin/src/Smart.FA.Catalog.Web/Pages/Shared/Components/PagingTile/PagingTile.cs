using Microsoft.AspNetCore.Mvc;
using Smart.Design.Razor.TagHelpers.Pagination;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Shared.Collections;

namespace Smart.FA.Catalog.Web.Pages.Shared.Components.PagingTile;

public class PagingTile : ViewComponent
{
    public IViewComponentResult Invoke(PagedList<Entity> list, Dictionary<string,string?> queryParameters)
    {
        var paginationSettings = GetPaginationSettings(list, GetQueryParametersAsString(queryParameters));
        return View(new PagingObject { List = list, PaginationSettings = paginationSettings });
    }

    private string GetQueryParametersAsString(Dictionary<string, string?> queryParameters)
    {
        var queryParametersString = string.Empty;
        foreach (var queryParameter in queryParameters)
        {
            if (!string.IsNullOrEmpty(queryParameter.Value))
            {
                queryParametersString += $"{queryParameter.Key}={queryParameter.Value}";
            }
        }

        return queryParametersString;
    }

    private PaginationSettings GetPaginationSettings(PagedList<Entity> list, string? searchKeyword)
    {
        return new PaginationSettings
        {
            NumberOfLinks = 5,
            PageNumber = list.CurrentPage,
            PageNumberParameterName = nameof(list.CurrentPage),
            TotalPages = list.TotalPages,
            QueryString = searchKeyword
        };
    }
}

public class PagingObject
{
    public PagedList<Entity> List { get; set; }
    public PaginationSettings PaginationSettings { get; set; }
}
