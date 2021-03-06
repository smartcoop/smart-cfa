using Smart.FA.Catalog.Application.SeedWork.Attributes;
using Smart.FA.Catalog.Application.UseCases.Commands;
using Smart.FA.Catalog.Core.Domain.Dto;
using Smart.FA.Catalog.Core.Domain.ValueObjects;
using Smart.FA.Catalog.Shared.Domain.Enumerations.Training;

namespace Smart.FA.Catalog.Web.Pages.Admin.Trainings.Create;

public class CreateTrainingViewModel
{
    public string? Title { get; set; }

    public List<int>? AttendanceTypeIds { get; set; }

    public List<int>? VatExemptionTypeIds { get; set; }

    public List<int>? TargetAudienceTypeIds { get; set; }

    public List<int>? TopicIds { get; set; }

    [Sanitized]
    public string? Goal { get; set; }

    [Sanitized]
    public string? Methodology { get; set; }

    [Sanitized]
    public string? PracticalModalities { get; set; }

    public bool IsDraft { get; set; }

    public bool IsGivenBySmart { get; set; }
}

public static class CreateTrainingViewModelMapping
{
    public static CreateTrainingRequest MapToRequest(this CreateTrainingViewModel model, int trainerId, Language language)
        => new()
        {
            DetailsDto = new
                TrainingLocalizedDetailsDto
                (
                    model.Title
                    , model.Goal
                    , language.Value
                    , model.Methodology
                    , model.PracticalModalities
                ),
            TrainerId = trainerId,
            VatExemptionTypes = VatExemptionType.FromValues(model.VatExemptionTypeIds ?? new()),
            TargetAudiences = TargetAudienceType.FromValues(model.TargetAudienceTypeIds ?? new()),
            AttendanceTypes = AttendanceType.FromValues(model.AttendanceTypeIds ?? new()),
            Topics = Topic.FromValues(model.TopicIds ?? new()),
            IsDraft = model.IsDraft,
            IsGivenBySmart = model.IsGivenBySmart
        };
}
