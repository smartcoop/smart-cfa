using Core.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Web.Pages.Shared.Components.AdminTrainingTile;

[ViewComponent(Name = "AdminTrainingTile")]
public class AdminTrainingCardViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IGrouping<int, TrainingDto> model)
    {
        return View(model);
    }
}
