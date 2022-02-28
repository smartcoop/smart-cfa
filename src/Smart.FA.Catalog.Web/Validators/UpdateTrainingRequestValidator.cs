using Application.UseCases.Commands;
using Core.Domain.Enumerations;
using Core.Domain.Validators;
using Core.SeedWork;
using Microsoft.Extensions.Localization;
using FluentValidation;
using Web.Pages.Admin.Trainings.Create;
using Web.Pages.Admin.Trainings.Update;

namespace Web.Validators;

public class UpdateTrainingViewModelValidator : AbstractValidator<UpdateTrainingViewModel>
{
    public UpdateTrainingViewModelValidator(IStringLocalizer<CreateTrainingViewModel> localizer)
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