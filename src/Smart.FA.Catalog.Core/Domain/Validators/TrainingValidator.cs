using FluentValidation;
using Smart.FA.Catalog.Core.Exceptions;

namespace Smart.FA.Catalog.Core.Domain.Validators;

public class TrainingValidator: AbstractValidator<Training>
{
    public TrainingValidator()
    {
        RuleFor(request => request.Identities)
            .NotEmpty().WithMessage(Errors.General.MissingField("identity").Message);
        RuleFor(request => request.Targets)
            .NotEmpty().WithMessage(Errors.General.MissingField("target").Message);
        RuleFor(request => request.Topics)
            .NotEmpty().WithMessage(Errors.General.MissingField("topic").Message);
        RuleFor(request => request.TrainerAssignments)
            .NotEmpty().WithMessage(Errors.General.MissingField("trainer").Message);
        RuleFor(request => request.TrainerAssignments)
            .NotEmpty().WithMessage(Errors.General.MissingField("trainer").Message);
        RuleFor(request => request.Details)
            .NotEmpty().WithMessage(Errors.General.MissingField("description").Message)
            .ForEach(detail => detail.SetValidator(new TrainingDetailValidation()));
    }
}
