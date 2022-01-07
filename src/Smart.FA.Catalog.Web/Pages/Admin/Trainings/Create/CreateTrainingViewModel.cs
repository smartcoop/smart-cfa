using Application.UseCases.Commands;
using Core.Domain;
using Core.Domain.Dto;
using Core.Domain.Enumerations;
using Core.SeedWork;

namespace Api.Pages.Admin.Trainings.Create;

public class CreateTrainingViewModel
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
    public static CreateTrainingRequest MapToRequest(this CreateTrainingViewModel model, TrainerDto trainer)
        => new()
        {
            Detail =
                new TrainingDetailDto
                {
                    Goal = model.Goal,
                    Language = trainer.DefaultLanguage.Value,
                    Methodology = model.Methodology,
                    Title = model.Title
                },
            TrainerId = trainer.Id,
            Types = Enumeration.FromValues<TrainingType>(model.TrainingTypeIds ?? new ()),
            TargetAudiences = Enumeration.FromValues<TrainingTargetAudience>(model.TargetAudienceIds ?? new()),
            SlotNumberTypes = Enumeration.FromValues<TrainingSlotNumberType>(model.SlotNumberTypeIds ?? new())
        };
}
