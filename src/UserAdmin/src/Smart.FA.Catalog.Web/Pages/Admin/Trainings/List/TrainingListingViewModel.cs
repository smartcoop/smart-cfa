using Smart.Design.Razor.TagHelpers.Pill;
using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Web.Pages.Admin.Trainings.List;

public record TrainingListingViewModel
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public List<string>? Tags { get; set; }

    public TrainingStatusType? Status { get; set; }

    public PillStatus PillStatus { get; set; }
}

public static class StatusExtension
{
    public static string DisplayStatusName(this TrainingStatusType trainingStatusType)
    {
        if (Equals(trainingStatusType, TrainingStatusType.Draft))
            return CatalogResources.Draft;

        if (Equals(trainingStatusType, TrainingStatusType.WaitingForValidation))
            return CatalogResources.PendingValidation;

        if (Equals(trainingStatusType, TrainingStatusType.Validated))
            return CatalogResources.Validated;

        return string.Empty;
    }
}
