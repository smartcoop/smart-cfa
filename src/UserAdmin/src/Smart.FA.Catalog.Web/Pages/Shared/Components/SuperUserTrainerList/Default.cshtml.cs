using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Core.Domain;

namespace Smart.FA.Catalog.Web.Pages.Shared.Components.SuperUserTrainerList;

public class SuperUserTrainerList : ViewComponent
{
    public IViewComponentResult Invoke(Trainer trainer)
    {
        return View(trainer);
    }
}

