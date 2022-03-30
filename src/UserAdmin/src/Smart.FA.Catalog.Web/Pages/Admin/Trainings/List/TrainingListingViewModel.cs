using Smart.Design.Razor.TagHelpers.Pill;
using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Web.Pages.Admin.Trainings.List;

public record TrainingListingViewModel
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public List<string>? Tags { get; set; }

    public TrainingStatus? Status { get; set; }

    public PillStatus PillStatus { get; set; }
}

public static class StatusExtension
{
    public static string DisplayStatusName(this TrainingStatus trainingStatus)
    {
        if (Equals(trainingStatus, TrainingStatus.Draft))
            return CatalogResources.Draft;

        if (Equals(trainingStatus, TrainingStatus.WaitingForValidation))
            return CatalogResources.PendingValidation;

        if (Equals(trainingStatus, TrainingStatus.Validated))
            return CatalogResources.Validated;

        return string.Empty;
    }
}
