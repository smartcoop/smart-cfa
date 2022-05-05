using FluentValidation;
using Microsoft.Extensions.Localization;
using Smart.FA.Catalog.Core.Domain;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Web.Pages.Admin.Trainings.Create;

namespace Smart.FA.Catalog.Web.Validators;

/// <summary>
/// Validates the request to create a <see cref="Training" />.
/// </summary>
public class CreateTrainingRequestValidator : AbstractValidator<CreateTrainingViewModel>
{
    private readonly IUserIdentity _userIdentity;

    public CreateTrainingRequestValidator(IStringLocalizer<CatalogResources> localizer, IUserIdentity userIdentity)
    {
        _userIdentity = userIdentity;
        RuleFor(viewModel => viewModel.Title)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage(CatalogResources.TrainingTitleIsRequired);

        RuleFor(viewModel => viewModel.Methodology)
            .MaximumLength(1000)
            .WithMessage(CatalogResources.Max1000Characters);

        RuleFor(viewModel => viewModel.Goal)
            .MaximumLength(1000)
            .WithMessage(CatalogResources.Max1000Characters);

        RuleFor(viewModel => viewModel.PracticalModalities)
            .MaximumLength(1000)
            .WithMessage(CatalogResources.Max1000Characters);

        RuleFor(viewModel => viewModel.PracticalModalities)
            .MaximumLength(1000)
            .WithMessage(CatalogResources.Max1000Characters);

        RuleFor(viewModel => viewModel.IsGivenBySmart)
            .Must(BeSuperUser)
            .When(IsMarkedAsGivenBySmart)
            .WithMessage(CatalogResources.SuperUserPermissionToSetSmartTrainingType);

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
            .WithMessage(CatalogResources.Min30Char);

        RuleFor(viewModel => viewModel.Goal)
            .NotEmpty()
            .WithMessage(CatalogResources.FieldRequired)
            .MinimumLength(30)
            .WithMessage(CatalogResources.Min30Char);

        RuleFor(viewModel => viewModel.PracticalModalities)
            .NotEmpty()
            .WithMessage(CatalogResources.FieldRequired)
            .MinimumLength(30)
            .WithMessage(CatalogResources.Min30Char);

        RuleFor(viewModel => viewModel.AttendanceTypeIds)
            .NotEmpty()
            .WithMessage(CatalogResources.FieldRequired);

        RuleFor(viewModel => viewModel.VatExemptionTypeIds)
            .NotEmpty()
            .WithMessage(CatalogResources.PleaseSelectOneOption);

        RuleFor(viewModel => viewModel.TargetAudienceTypeIds)
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

    private bool BeSuperUser(bool _) => _userIdentity.IsSuperUser;

    private bool IsMarkedAsGivenBySmart(CreateTrainingViewModel viewModel) => viewModel.IsGivenBySmart;

}
