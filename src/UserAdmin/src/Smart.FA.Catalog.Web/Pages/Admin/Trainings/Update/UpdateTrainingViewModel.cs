using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Application.UseCases.Queries;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Shared.Domain.Enumerations;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Web.Pages.Admin.Trainings.Update;

public class UpdateTrainingViewModel
{
    public string? Title { get; init; }
    public List<int>? SlotNumberTypeIds { get; init; }
    public List<int>? TrainingTypeIds { get; init; }
    public List<int>? TargetAudienceIds { get; init; }
    public List<int>? TopicIds { get; init; }
    public string? Goal { get; init; }
    public string? Methodology { get; init; }
    public bool IsDraft { get; set; }
}

public static class EditTrainingViewModelMapping
{
    public static UpdateTrainingRequest MapToUpdateRequest(this UpdateTrainingViewModel model, string language, int trainingId, int trainerId)
        => new()
        {
            Detail =
                new TrainingDetailDto
                (
                    model.Title
                    , model.Goal
                    , language
                    , model.Methodology
                ),
            TrainingId = trainingId
            , Types = Enumeration.FromValues<TrainingType>(model.TrainingTypeIds ?? new())
            , TargetAudiences = Enumeration.FromValues<TrainingTargetAudience>(model.TargetAudienceIds ?? new())
            , SlotNumberTypes = Enumeration.FromValues<TrainingSlotNumberType>(model.SlotNumberTypeIds ?? new())
            , Topics = Enumeration.FromValues<TrainingTopic>(model.TopicIds ?? new())
            , TrainerIds = new List<int>{trainerId},
        };


    public static UpdateTrainingViewModel MapGetToResponse(this GetTrainingFromIdResponse model, Language language)
    {
        var detail = model.Training!.Details.FirstOrDefault(detail => detail.Language == language);
        UpdateTrainingViewModel response = new()
        {
            Goal = detail?.Goal,
            Title = detail?.Title,
            Methodology = detail?.Methodology,
            TargetAudienceIds = model.Training.Targets.Select(target => target.TrainingTargetAudienceId).ToList(),
            TrainingTypeIds = model.Training.Identities.Select(identity => identity.TrainingTypeId).ToList(),
            SlotNumberTypeIds = model.Training.Slots.Select(slot => slot.TrainingSlotTypeId).ToList(),
            TopicIds = model.Training.Topics.Select(topic => topic.TrainingTopicId).ToList()
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
            TargetAudienceIds = model.Training.Targets.Select(target => target.TrainingTargetAudienceId).ToList(),
            TrainingTypeIds = model.Training.Identities.Select(identity => identity.TrainingTypeId).ToList(),
            SlotNumberTypeIds = model.Training.Slots.Select(slot => slot.TrainingSlotTypeId).ToList()
        };

        return response;
    }
}
