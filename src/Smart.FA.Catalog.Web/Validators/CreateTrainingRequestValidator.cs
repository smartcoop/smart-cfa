using FluentValidation;
using Microsoft.Extensions.Localization;
using Smart.FA.Catalog.Core.Domain.Enumerations;
using Smart.FA.Catalog.Core.Domain.Validators;
using Smart.FA.Catalog.Core.SeedWork;
using Smart.FA.Catalog.Web.Pages.Admin.Trainings.Create;

namespace Smart.FA.Catalog.Web.Validators;

public class CreateTrainingViewModelValidator : AbstractValidator<CreateTrainingViewModel>
{
    public CreateTrainingViewModelValidator(IStringLocalizer<CreateTrainingViewModelValidator> localizer)
    {
        CustomValidators.NotEmpty(RuleFor(request => request.Title)).WithMessage(localizer[Errors.TrainingViewModel.EmptyTitle().Code]);
        When(request =>  request.IsDraft is not true,
            () => CustomValidators.NotEmpty(RuleFor(request => request.Methodology)).WithMessage(localizer[Errors.TrainingViewModel.EmptyMethodology().Code]));
        When(request =>  request.IsDraft is not true,
            () => CustomValidators.NotEmpty(RuleFor(request => request.Goal)).WithMessage(localizer[Errors.TrainingViewModel.EmptyGoal().Code]));
        When(request => request.IsDraft is not true,
            () => CustomValidators.NotEmpty(RuleFor(request => request.SlotNumberTypeIds)).WithMessage(localizer[Errors.TrainingViewModel.MissingSlotType().Code])
                .ForEach(types => types.MustBeEnumeration(Enumeration.FromValue<TrainingSlotNumberType>)));
        When(request => request.IsDraft is not true,
            () => CustomValidators.NotEmpty(RuleFor(request => request.TargetAudienceIds)).WithMessage(localizer[Errors.TrainingViewModel.MissingTargetAudience().Code])
                .ForEach(types => types.MustBeEnumeration(Enumeration.FromValue<TrainingTargetAudience>)));
        When(request =>  request.IsDraft is not true,
            () => CustomValidators.NotEmpty(RuleFor(request => request.TrainingTypeIds)).WithMessage(localizer[Errors.TrainingViewModel.MissingType().Code])
                .ForEach(types => types.MustBeEnumeration(Enumeration.FromValue<TrainingType>)));
    }
}
