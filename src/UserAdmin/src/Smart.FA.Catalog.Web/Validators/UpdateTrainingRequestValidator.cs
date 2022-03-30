using FluentValidation;
using Microsoft.Extensions.Localization;
using Smart.FA.Catalog.Web.Pages.Admin.Trainings.Update;

namespace Smart.FA.Catalog.Web.Validators;

/// <summary>
/// Validates the request to update a training.
/// </summary>
public class UpdateTrainingViewModelValidator : AbstractValidator<UpdateTrainingViewModel>
{
    public UpdateTrainingViewModelValidator(IStringLocalizer<CatalogResources> localizer)
    {
        RuleFor(viewModel => viewModel.Title)
            .NotEmpty()
            .WithMessage(CatalogResources.TrainingTitleIsRequired);

        // When we register a draft we are very permissive.
        // Only the title is required.
        // However to be able to publish more rules are required.
        When(IsNotDraft, ApplyRemainingRules);
    }

    private void ApplyRemainingRules()
    {
        RuleFor(viewModel => viewModel.Methodology)
            .NotEmpty()
            .WithMessage(CatalogResources.FieldRequired)
            .MinimumLength(30)
            .WithMessage(" ")
            .MaximumLength(500)
            .WithMessage(" ");

        RuleFor(viewModel => viewModel.Goal)
            .NotEmpty()
            .WithMessage(CatalogResources.FieldRequired)
            .MinimumLength(30)
            .WithMessage(" ")
            .MaximumLength(500)
            .WithMessage(" ");

        RuleFor(viewModel => viewModel.TopicIds)
            .NotEmpty()
            .WithMessage(CatalogResources.YouMustSelectedAtLeastOneTopic);

        RuleFor(viewModel => viewModel.TrainingTypeIds)
            .NotEmpty()
            .WithMessage(CatalogResources.PleaseSelectOneOption);

        RuleFor(viewModel => viewModel.SlotNumberTypeIds)
            .NotEmpty()
            .WithMessage(CatalogResources.PleaseSelectOneOption);

        RuleFor(viewModel => viewModel.TargetAudienceIds)
            .NotEmpty()
            .WithMessage(CatalogResources.PleaseSelectOneOption);
    }

    private bool IsNotDraft(UpdateTrainingViewModel viewModel)
    {
        return !viewModel.IsDraft;
    }
}
