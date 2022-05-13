using Microsoft.AspNetCore.Mvc;
using Smart.Design.Razor.TagHelpers.Pill;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Web.Pages.Shared.Components.AdminTrainingTile;

[ViewComponent(Name = "AdminTrainingTile")]
public class AdminTrainingCardViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(Training training, bool showDeleteButton = false)
    {
        return View(new AdminTrainingCardViewComponentModel
        {
            Training = training ?? throw new ArgumentNullException(nameof(training)),
            ShowDeleteButton = showDeleteButton
        });
    }
}

public class AdminTrainingCardViewComponentModel
{
    public Training Training { get; init; } = null!;

    public bool ShowDeleteButton { get; init; }

    public PillStatus PillStatus => Training.StatusType == TrainingStatusType.Published ? PillStatus.Success : PillStatus.Pending;
}
