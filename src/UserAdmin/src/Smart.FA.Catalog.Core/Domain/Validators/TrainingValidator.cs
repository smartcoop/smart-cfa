using FluentValidation;
using Smart.FA.Catalog.Core.Exceptions;

namespace Smart.FA.Catalog.Core.Domain.Validators;

public class TrainingValidator : AbstractValidator<Training>
{
    public TrainingValidator()
    {
        RuleFor(request => request.VatExemptionClaims)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("identity").Message);
        RuleFor(request => request.Targets)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("target").Message);
        RuleFor(request => request.Topics)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("topic").Message);
        RuleFor(request => request.Attendances)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("attendance").Message);
        RuleFor(request => request.TrainerAssignments)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("trainer").Message);
        RuleFor(request => request.Details)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("description").Message)
            .ForEach(details => details.SetValidator(new TrainingLocalizedDetailsValidator()));
    }
}
