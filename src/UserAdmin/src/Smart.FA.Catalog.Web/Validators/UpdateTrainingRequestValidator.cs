using FluentValidation;
using Microsoft.Extensions.Localization;
using Smart.FA.Catalog.Application.Extensions.FluentValidation;
using Smart.FA.Catalog.Core.Services;
using Smart.FA.Catalog.Web.Pages.Admin.Trainings.Update;

namespace Smart.FA.Catalog.Web.Validators;

/// <summary>
/// Validates the request to update a training.
/// </summary>
public class UpdateTrainingViewModelValidator : AbstractValidator<UpdateTrainingViewModel>
{
    private readonly IUserIdentity _userIdentity;

    public UpdateTrainingViewModelValidator(IUserIdentity userIdentity)
    {
        _userIdentity = userIdentity;

        RuleFor(viewModel => viewModel.Title)
            .NotEmpty()
            .WithMessage(CatalogResources.TrainingTitleIsRequired);

        RuleFor(viewModel => viewModel.Methodology)
            .MaximumHtmlInnerLength(1000)
            .WithMessage(CatalogResources.Max1000Characters);

        RuleFor(viewModel => viewModel.Goal)
            .MaximumHtmlInnerLength(1000)
            .WithMessage(CatalogResources.Max1000Characters);

        RuleFor(viewModel => viewModel.PracticalModalities)
            .MaximumHtmlInnerLength(1000)
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

    private void ApplyRemainingRules()
    {
        RuleFor(viewModel => viewModel.Methodology)
            .NotEmpty()
            .WithMessage(CatalogResources.FieldRequired)
            .MinimumHtmlInnerLength(30)
            .WithMessage(CatalogResources.Min30Char);

        RuleFor(viewModel => viewModel.Goal)
            .NotEmpty()
            .WithMessage(CatalogResources.FieldRequired)
            .MinimumHtmlInnerLength(30)
            .WithMessage(CatalogResources.Min30Char);

        RuleFor(viewModel => viewModel.PracticalModalities)
            .NotEmpty()
            .WithMessage(CatalogResources.FieldRequired)
            .MinimumHtmlInnerLength(30)
            .WithMessage(CatalogResources.Min30Char);

        RuleFor(viewModel => viewModel.TopicIds)
            .NotEmpty()
            .WithMessage(CatalogResources.YouMustSelectedAtLeastOneTopic);

        RuleFor(viewModel => viewModel.VatExemptionTypeIds)
            .NotEmpty()
            .WithMessage(CatalogResources.PleaseSelectOneOption);

        RuleFor(viewModel => viewModel.AttendanceTypeIds)
            .NotEmpty()
            .WithMessage(CatalogResources.PleaseSelectOneOption);

        RuleFor(viewModel => viewModel.TargetAudienceTypeIds)
            .NotEmpty()
            .WithMessage(CatalogResources.PleaseSelectOneOption);
    }

    private bool IsNotDraft(UpdateTrainingViewModel viewModel)
    {
        return !viewModel.IsDraft;
    }

    private bool BeSuperUser(bool _) => _userIdentity.IsSuperUser;

    private bool IsMarkedAsGivenBySmart(UpdateTrainingViewModel viewModel) => viewModel.IsGivenBySmart;
}
