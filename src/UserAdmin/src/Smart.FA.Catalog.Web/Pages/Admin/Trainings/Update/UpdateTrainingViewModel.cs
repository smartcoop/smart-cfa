using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Shared.Domain.Enumerations;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Web.Pages.Admin.Trainings.Update;

public class UpdateTrainingViewModel
{
    public string? Title { get; init; }
    public List<int>? AttendanceTypeIds { get; init; }
    public List<int>? VatExemptionClaimIds { get; init; }
    public List<int>? TargetAudienceTypeIds { get; init; }
    public List<int>? TopicIds { get; init; }
    public string? Goal { get; init; }
    public string? Methodology { get; init; }
    public bool IsDraft { get; set; }
    public string? PracticalModalities { get; set; }
}

public static class EditTrainingViewModelMapping
{
    public static UpdateTrainingRequest MapToUpdateRequest(this UpdateTrainingViewModel model, string language, int trainingId, int trainerId)
        => new()
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
            TrainingId = trainingId
            , VatExemptionTypes = VatExemptionType.FromValues(model.VatExemptionClaimIds ?? new())
            , TargetAudienceTypes = TargetAudienceType.FromValues(model.TargetAudienceTypeIds ?? new())
            , AttendanceTypes = AttendanceType.FromValues(model.AttendanceTypeIds ?? new())
            , Topics = Topic.FromValues(model.TopicIds ?? new())
            , TrainerIds = new List<int>{trainerId},
        };


    public static UpdateTrainingViewModel MapGetToResponse(this GetTrainingFromIdResponse model, Language language)
    {
        var details = model.Training!.Details.FirstOrDefault(localizedDetails => localizedDetails.Language == language);
        UpdateTrainingViewModel response = new()
        {
            Goal = details?.Goal,
            Title = details?.Title,
            Methodology = details?.Methodology,
            PracticalModalities = details?.PracticalModalities,
            TargetAudienceTypeIds = model.Training.Targets.Select(target => target.TargetAudienceType.Id).ToList(),
            VatExemptionClaimIds = model.Training.VatExemptionClaims.Select(vatExemptionClaim => vatExemptionClaim.VatExemptionType.Id).ToList(),
            AttendanceTypeIds = model.Training.Attendances.Select(attendance => attendance.AttendanceType.Id).ToList(),
            TopicIds = model.Training.Topics.Select(topic => topic.TopicId).ToList()
        };

        return response;
    }

    public static UpdateTrainingViewModel MapUpdateToResponse(this UpdateTrainingResponse model, Language language)
    {
        var detail = model.Training.Details.FirstOrDefault(detail => detail.Language == language);
        UpdateTrainingViewModel response = new()
        {
            Goal = detail?.Goal,
            Title = detail?.Title,
            Methodology = detail?.Methodology,
            PracticalModalities = detail?.PracticalModalities,
            TargetAudienceTypeIds = model.Training.Targets.Select(target => target.TargetAudienceType.Id).ToList(),
            VatExemptionClaimIds = model.Training.VatExemptionClaims.Select(vatExemptionClaim => vatExemptionClaim.VatExemptionType.Id).ToList(),
            AttendanceTypeIds = model.Training.Attendances.Select(attendance => attendance.AttendanceType.Id).ToList()
        };

        return response;
    }
}
