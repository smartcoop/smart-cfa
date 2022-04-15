using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Core.Domain.Dto;

namespace Smart.FA.Catalog.Web.Pages.Shared.Components.AdminTrainingTile;

[ViewComponent(Name = "AdminTrainingTile")]
public class AdminTrainingCardViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(TrainingDto model)
    {
        return View(model ?? throw new ArgumentNullException(nameof(model)));
    }
}
