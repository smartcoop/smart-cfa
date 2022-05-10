using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Core.Domain.Dto;

namespace Smart.FA.Catalog.Web.Pages.Shared.Components.AdminTrainingTile;

[ViewComponent(Name = "AdminTrainingTile")]
public class AdminTrainingCardViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(TrainingDto trainingDto, bool showDeleteButton = false)
    {
        return View(new AdminTrainingCardViewComponentModel()
        {
            TrainingDto      = trainingDto ?? throw new ArgumentNullException(nameof(trainingDto)),
            ShowDeleteButton = showDeleteButton
        });
    }
}

public class AdminTrainingCardViewComponentModel
{
    public TrainingDto? TrainingDto { get; init; }

    public bool ShowDeleteButton { get; init; }
}
