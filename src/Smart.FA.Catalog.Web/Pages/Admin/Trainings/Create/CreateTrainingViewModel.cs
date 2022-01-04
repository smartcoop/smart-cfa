using Application.UseCases.Commands;
using Application.UseCases.Dto;
using Core.Domain;
using Core.SeedWork;

namespace Api.Pages.Admin.Trainings.Create;

public class CreateTrainingViewModel
{
    public string? Title { get; set; }

    public int  SlotNumberTypeId { get; set; }

    public List<int>? TrainingTypeIds { get; set; }

    public List<int>? TargetAudienceIds { get; set; }

    public string? Goal { get; set; }

    public string? Methodology { get; set; }

    public string? Address { get; set; }
}

public static class CreateTrainingViewModelMapping
{
    public static CreateTrainingRequest MapToRequest(this CreateTrainingViewModel model, Trainer trainer)
        => new()
        {
            Detail =
                new TrainingDetailDto
                {
                    Goal = model.Goal,
                    Language = trainer.DefaultLanguage,
                    Methodology = model.Methodology,
                    Title = model.Title
                },
            TrainerId = trainer.Id,
            Types = Enumeration.FromValues<TrainingType>(model.TrainingTypeIds),
            TargetAudiences = Enumeration.FromValues<TrainingTargetAudience>(model.TargetAudienceIds),
            SlotNumberType = Enumeration.FromValue<TrainingSlotNumberType>(model.SlotNumberTypeId),
        };
}
