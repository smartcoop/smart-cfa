using FluentValidation;
using Smart.FA.Catalog.Core.Exceptions;

namespace Smart.FA.Catalog.Core.Domain.Validators;

public class TrainingValidator: AbstractValidator<Training>
{
    public TrainingValidator()
    {
        RuleFor(request => request.Identities)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("identity").Message);
        RuleFor(request => request.Targets)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("target").Message);
        RuleFor(request => request.Topics)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("topic").Message);
        RuleFor(request => request.TrainerAssignments)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("trainer").Message);
        RuleFor(request => request.TrainerAssignments)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("trainer").Message);
        RuleFor(request => request.Details)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("description").Message)
            .ForEach(detail => detail.SetValidator(new TrainingDetailValidation()));
    }
}
