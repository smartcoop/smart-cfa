using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Shared.Domain.Enumerations;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Web.Pages.Admin.Trainings.Create;

public class CreateTrainingViewModel
{
    public string? Title { get; set; }
    public List<int>? SlotNumberTypeIds { get; set; }
    public List<int>? TrainingTypeIds { get; set; }
    public List<int>? TargetAudienceIds { get; set; }
    public List<int>? TopicIds { get; set; }
    public string? Goal { get; set; }
    public string? Methodology { get; set; }
    public string? PracticalModalities { get; set; }
    public bool IsDraft { get; set; }
}

public static class CreateTrainingViewModelMapping
{
    public static CreateTrainingRequest MapToRequest(this CreateTrainingViewModel model, int trainerId,
        Language language)
        => new()
        {
            Detail = new
                TrainingDetailDto
                (
                    model.Title
                    , model.Goal
                    , language.Value
                    , model.Methodology
                    , model.PracticalModalities
                ),
            TrainerId = trainerId,
            Types = Enumeration.FromValues<TrainingType>(model.TrainingTypeIds ?? new()),
            TargetAudiences = Enumeration.FromValues<TrainingTargetAudience>(model.TargetAudienceIds ?? new()),
            SlotNumberTypes = Enumeration.FromValues<TrainingSlotNumberType>(model.SlotNumberTypeIds ?? new()),
            Topics = Enumeration.FromValues<TrainingTopic>(model.TopicIds ?? new()),
            IsDraft = model.IsDraft
        };
}
