using Microsoft.AspNetCore.Mvc;
using Smart.Design.Razor.TagHelpers.Pill;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Infrastructure.Persistence;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Web.Pages.Shared.Components.AdminTrainingTile;

[ViewComponent(Name = "AdminTrainingTile")]
public class AdminTrainingCardViewComponent : ViewComponent
{
    private readonly CatalogContext _catalogContext;
    private Training _training = null!;

    public AdminTrainingCardViewComponent(CatalogContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task<IViewComponentResult> InvokeAsync(Training training, bool showDeleteButton = false, bool showTrainerName = false)
    {
        _training = training;

        return View(new AdminTrainingCardViewComponentModel
        {
            Training = training ?? throw new ArgumentNullException(nameof(training)),
            ShowDeleteButton = showDeleteButton,
            TrainerName = showTrainerName ? await GetTrainerNameAsync() : string.Empty
        });
    }

    private async Task<string> GetTrainerNameAsync()
    {
        // Lazy-loading will trigger a n+1 problem if trainers were not included. So make sure to do so.
        var trainingCreator =  await _catalogContext.Trainers.FindAsync(_training.CreatedBy);

        return trainingCreator?.Name ?? string.Empty;
    }
}

public class AdminTrainingCardViewComponentModel
{
    public Training Training { get; init; } = null!;

    public string? TrainerName { get; set; }

    public bool ShowDeleteButton { get; init; }

    public PillStatus PillStatus => Training.StatusType == TrainingStatusType.Published ? PillStatus.Success : PillStatus.Pending;
}
