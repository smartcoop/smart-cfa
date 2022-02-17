using Core.Exceptions;
using FluentValidation;

namespace Core.Domain.Validators;

public class TrainingDetailValidation: AbstractValidator<TrainingDetail>
{
    public TrainingDetailValidation(bool isDraft)
    {
        RuleFor(request => request.Title)
            .NotEmpty().WithMessage(Errors.General.MissingField("title").Message);
        When(_ => isDraft, () => RuleFor(request => request.Language)
            .NotEmpty().WithMessage(Errors.General.MissingField("language").Message));
        When(_ => isDraft,() => RuleFor(request => request.Goal)
            .NotEmpty().WithMessage(Errors.General.MissingField("goal").Message));
        When(_ => isDraft,() => RuleFor(request => request.Methodology)
            .NotEmpty().WithMessage(Errors.General.MissingField("methodology").Message));
    }
}
