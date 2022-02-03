using Application.UseCases.Commands;
using Application.UseCases.Queries;
using Core.Domain;
using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.SeedWork;

namespace Api.Pages.Admin.Trainings.Edit;

public class EditTrainingViewModel
{
    public string? Title { get; init; }

    public List<int>? SlotNumberTypeIds { get; init; }

    public List<int>? TrainingTypeIds { get; init; }

    public List<int>? TargetAudienceIds { get; init; }

    public string? Goal { get; init; }

    public string? Methodology { get; init; }

    // public string? Address { get; set; }
}

public static class EditTrainingViewModelMapping
{
    public static UpdateTrainingRequest MapToUpdateRequest(this EditTrainingViewModel model, string language, int trainingId, int trainerId)
        => new()
        {
            Detail =
                new TrainingDetailDto
                (
                    model.Title,
                    model.Goal,
                    language,
                     model.Methodology
                ),
            TrainingId = trainingId,
            Types = Enumeration.FromValues<TrainingType>(model.TrainingTypeIds ?? new()),
            TargetAudiences = Enumeration.FromValues<TrainingTargetAudience>(model.TargetAudienceIds ?? new()),
            SlotNumberTypes = Enumeration.FromValues<TrainingSlotNumberType>(model.SlotNumberTypeIds ?? new()),
            TrainerIds = new List<int>{trainerId}
        };


    public static EditTrainingViewModel MapGetToResponse(this GetTrainingFromIdResponse model, Language language)
    {
        var detail = model.Training!.Details.FirstOrDefault(detail => detail.Language == language);
        EditTrainingViewModel response = new()
        {
            Goal = detail?.Goal,
            Title = detail?.Title,
            Methodology = detail?.Methodology,
            TargetAudienceIds = model.Training.Targets.Select(target => target.TrainingTargetAudienceId).ToList(),
            TrainingTypeIds = model.Training.Identities.Select(identity => identity.TrainingTypeId).ToList(),
            SlotNumberTypeIds = model.Training.Slots.Select(slot => slot.TrainingSlotTypeId).ToList()
        };

        return response;
    }

    public static EditTrainingViewModel MapUpdateToResponse(this UpdateTrainingResponse model, Language language)
    {
        var detail = model.Training.Details.FirstOrDefault(detail => detail.Language == language);
        EditTrainingViewModel response = new()
        {
            Goal = detail?.Goal,
            Title = detail?.Title,
            Methodology = detail?.Methodology,
            TargetAudienceIds = model.Training.Targets.Select(target => target.TrainingTargetAudienceId).ToList(),
            TrainingTypeIds = model.Training.Identities.Select(identity => identity.TrainingTypeId).ToList(),
            SlotNumberTypeIds = model.Training.Slots.Select(slot => slot.TrainingSlotTypeId).ToList()
        };

        return response;
    }

    public static GetTrainingValidationErrorsRequest MapDraftToRequest(this EditTrainingViewModel model, int trainerId, Language language)
        => new()
        {
            Detail = new
                TrainingDetailDto
                (
                    model.Title,
                    model.Goal,
                    language.Value,
                    model.Methodology
                ),
            TrainerId = trainerId,
            Types = Enumeration.FromValues<TrainingType>(model.TrainingTypeIds ?? new()),
            TargetAudiences = Enumeration.FromValues<TrainingTargetAudience>(model.TargetAudienceIds ?? new()),
            SlotNumberTypes = Enumeration.FromValues<TrainingSlotNumberType>(model.SlotNumberTypeIds ?? new())
        };
}
