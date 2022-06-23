using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;
using Smart.FA.Catalog.Web.Pages.Admin.Trainings.Create;

namespace Smart.FA.Catalog.Web.Pages.Admin.Trainings.Update;

public class UpdateTrainingViewModel : CreateTrainingViewModel
{
}

public static class EditTrainingViewModelMapping
{
    public static UpdateTrainingRequest MapToRequest(this UpdateTrainingViewModel model, string language, int trainingId, int trainerId) =>
        new()
        {
            DetailsDto =
                new TrainingLocalizedDetailsDto
                (
                    model.Title
                    , model.Goal
                    , language
                    , model.Methodology
                    , model.PracticalModalities
                ),
            TrainingId = trainingId,
            VatExemptionTypes = VatExemptionType.FromValues(model.VatExemptionTypeIds ?? new()),
            TargetAudienceTypes = TargetAudienceType.FromValues(model.TargetAudienceTypeIds ?? new()),
            AttendanceTypes = AttendanceType.FromValues(model.AttendanceTypeIds ?? new()),
            Topics = Topic.FromValues(model.TopicIds ?? new()),
            TrainerIds = new List<int> { trainerId },
            IsGivenBySmart = model.IsGivenBySmart,
            IsDraft = model.IsDraft
        };


    public static UpdateTrainingViewModel MapToViewModel(this GetTrainingFromIdResponse model, Language language)
    {
        var details = model.Training!.Details.FirstOrDefault(localizedDetails => localizedDetails.Language == language);
        UpdateTrainingViewModel response = new()
        {
            Goal = details?.Goal,
            Title = details?.Title,
            Methodology = details?.Methodology,
            PracticalModalities = details?.PracticalModalities,
            TargetAudienceTypeIds = model.Training.Targets.Select(target => target.TargetAudienceType.Id).ToList(),
            VatExemptionTypeIds = model.Training.VatExemptionClaims.Select(vatExemptionClaim => vatExemptionClaim.VatExemptionType.Id).ToList(),
            AttendanceTypeIds = model.Training.Attendances.Select(attendance => attendance.AttendanceType.Id).ToList(),
            TopicIds = model.Training.Topics.Select(topic => topic.Topic.Id).ToList(),
            IsGivenBySmart = model.Training.IsGivenBySmart
        };

        return response;
    }

    public static UpdateTrainingViewModel MapToViewModel(this UpdateTrainingResponse model, Language language)
    {
        var detail = model.Training.Details.FirstOrDefault(detail => detail.Language == language);
        UpdateTrainingViewModel response = new()
        {
            Goal = detail?.Goal,
            Title = detail?.Title,
            Methodology = detail?.Methodology,
            PracticalModalities = detail?.PracticalModalities,
            TargetAudienceTypeIds = model.Training.Targets.Select(target => target.TargetAudienceType.Id).ToList(),
            VatExemptionTypeIds = model.Training.VatExemptionClaims.Select(vatExemptionClaim => vatExemptionClaim.VatExemptionType.Id).ToList(),
            AttendanceTypeIds = model.Training.Attendances.Select(attendance => attendance.AttendanceType.Id).ToList(),
            IsGivenBySmart = model.Training.IsGivenBySmart,
            IsDraft = model.Training.StatusType == TrainingStatusType.Draft
        };

        return response;
    }
}
