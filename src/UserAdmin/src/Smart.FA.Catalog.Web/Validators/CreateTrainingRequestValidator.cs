using FluentValidation;
using Microsoft.Extensions.Localization;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Web.Pages.Admin.Trainings.Create;

namespace Smart.FA.Catalog.Web.Validators;

/// <summary>
/// Validates the request to create a <see cref="Training" />.
/// </summary>
public class CreateTrainingViewModelValidator : AbstractValidator<CreateTrainingViewModel>
{
    public CreateTrainingViewModelValidator(IStringLocalizer<CatalogResources> localizer)
    {
        RuleFor(viewModel => viewModel.Title)
            .NotEmpty()
            .WithMessage(CatalogResources.TrainingTitleIsRequired);

        // When we register a draft we are very permissive.
        // Only the title is required.
        // However to be able to publish more rules are required.
        When(IsNotDraft, ApplyRemainingRules);
    }

    /// <summary>
    /// Add the necessary validation rules to publish a <see cref="Training" />
    /// </summary>
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

        RuleFor(viewModel => viewModel.SlotNumberTypeIds)
            .NotEmpty()
            .WithMessage(CatalogResources.FieldRequired);

        RuleFor(viewModel => viewModel.TrainingTypeIds)
            .NotEmpty()
            .WithMessage(CatalogResources.PleaseSelectOneOption);

        RuleFor(viewModel => viewModel.TargetAudienceIds)
            .NotEmpty()
            .WithMessage(CatalogResources.SelectOneTrainingTargetAudience);

        RuleFor(viewModel => viewModel.TopicIds)
            .NotEmpty()
            .WithMessage(CatalogResources.YouMustSelectedAtLeastOneTopic);
    }

    private bool IsNotDraft(CreateTrainingViewModel viewModel)
    {
        return !viewModel.IsDraft;
    }
}
