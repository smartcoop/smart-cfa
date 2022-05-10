using Microsoft.AspNetCore.Mvc;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Domain.Dto;

namespace Smart.FA.Catalog.Web.Pages.Shared.Components.AdminTrainingTile;

[ViewComponent(Name = "AdminTrainingTile")]
public class AdminTrainingCardViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(Training training, bool showDeleteButton = false)
    {
        return View(new AdminTrainingCardViewComponentModel
        {
            Training      = training ?? throw new ArgumentNullException(nameof(training)),
            ShowDeleteButton = showDeleteButton
        });
    }
}

public class AdminTrainingCardViewComponentModel
{
    public Training? Training { get; init; }

    public bool ShowDeleteButton { get; init; }
}
