using Core.Domain;
using Core.Domain.Enumerations;
using Smart.Design.Razor.TagHelpers.Pill;

namespace Api.Pages.Admin.Trainings.List;

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
    public static string DisplayStatus(this TrainingStatus trainingStatus)
    {
        if (Equals(trainingStatus, TrainingStatus.Draft))
            return "Brouillon";

        if (Equals(trainingStatus, TrainingStatus.WaitingForValidation))
            return "En attente de validation";

        return Equals(trainingStatus, TrainingStatus.Validated) ? "Validé" : string.Empty;
    }
}
