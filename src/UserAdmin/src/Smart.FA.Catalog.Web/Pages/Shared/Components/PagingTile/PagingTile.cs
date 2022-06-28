using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Shared.Collections;

namespace Smart.FA.Catalog.Web.Pages.Shared.Components.PagingTile;

public class PagingTile : ViewComponent
{
    public IViewComponentResult Invoke(PagedList<Entity> list, string route)
    {
        return View(new PagingObject { List = list, Route = route });
    }
}

public class PagingObject
{
    public PagedList<Entity> List { get; set; }
    public string Route { get; set; }
}
