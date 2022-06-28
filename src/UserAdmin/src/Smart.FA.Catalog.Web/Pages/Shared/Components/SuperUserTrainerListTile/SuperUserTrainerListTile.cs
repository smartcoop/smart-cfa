using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Core.Domain;

namespace Smart.FA.Catalog.Web.Pages.Shared.Components.SuperUserTrainerListTile;

public class SuperUserTrainerListTile : ViewComponent
{
    public IViewComponentResult Invoke(Trainer trainer)
    {
        return View(trainer);
    }
}
