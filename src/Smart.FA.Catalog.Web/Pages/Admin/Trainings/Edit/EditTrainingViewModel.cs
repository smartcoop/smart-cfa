using Application.UseCases.Commands;
using Application.UseCases.Queries;
using Core.Domain;
using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.SeedWork;

namespace Api.Pages.Admin.Trainings.Edit;

public class EditTrainingViewModel
{
    public string? Title { get; set; }

    public List<int>? SlotNumberTypeIds { get; set; }

    public List<int>? TrainingTypeIds { get; set; }

    public List<int>? TargetAudienceIds { get; set; }

    public string? Goal { get; set; }

    public string? Methodology { get; set; }

    public string? Address { get; set; }
}

public static class EditTrainingViewModelMapping
{
    public static UpdateTrainingRequest MapToUpdateRequest(this EditTrainingViewModel model, string language, int trainingId, int trainerId)
        => new()
        {
            Detail =
                new TrainingDetailDto
                {
                    Goal = model.Goal,
                    Language = language,
                    Methodology = model.Methodology,
                    Title = model.Title
                },
            TrainingId = trainingId,
            Types = Enumeration.FromValues<TrainingType>(model.TrainingTypeIds),
            TargetAudiences = Enumeration.FromValues<TrainingTargetAudience>(model.TargetAudienceIds),
            SlotNumberTypes = Enumeration.FromValues<TrainingSlotNumberType>(model.SlotNumberTypeIds),
            TrainerIds = new List<int>{trainerId}
        };


    public static EditTrainingViewModel MapGetToResponse(this GetTrainingFromIdResponse model, string language)
    {
        var detail = model.Training.Details.FirstOrDefault(detail => detail.Language == language);
        EditTrainingViewModel response = new()
        {
            Goal = detail.Goal,
            Title = detail.Title,
            Methodology = detail.Methodology,
            TargetAudienceIds = model.Training.Targets.Select(target => target.TrainingTargetAudienceId).ToList(),
            TrainingTypeIds = model.Training.Identities.Select(identity => identity.TrainingTypeId).ToList(),
            SlotNumberTypeIds = model.Training.Slots.Select(slot => slot.TrainingSlotTypeId).ToList()
        };

        return response;
    }

    public static EditTrainingViewModel MapUpdateToResponse(this UpdateTrainingResponse model, string language)
    {
        var detail = model.Training.Details.FirstOrDefault(detail => detail.Language == language);
        EditTrainingViewModel response = new()
        {
            Goal = detail.Goal,
            Title = detail.Title,
            Methodology = detail.Methodology,
            TargetAudienceIds = model.Training.Targets.Select(target => target.TrainingTargetAudienceId).ToList(),
            TrainingTypeIds = model.Training.Identities.Select(identity => identity.TrainingTypeId).ToList(),
            SlotNumberTypeIds = model.Training.Slots.Select(slot => slot.TrainingSlotTypeId).ToList()
        };

        return response;
    }
}
