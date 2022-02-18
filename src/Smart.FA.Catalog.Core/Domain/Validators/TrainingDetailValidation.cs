using Core.Exceptions;
using FluentValidation;

namespace Core.Domain.Validators;

public class TrainingDetailValidation: AbstractValidator<TrainingDetail>
{
    public TrainingDetailValidation()
    {
        RuleFor(request => request.Title)
            .NotEmpty().WithMessage(Errors.General.MissingField("title").Message);
        RuleFor(request => request.Language)
            .NotEmpty().WithMessage(Errors.General.MissingField("language").Message);
        RuleFor(request => request.Goal)
            .NotEmpty().WithMessage(Errors.General.MissingField("goal").Message);
        RuleFor(request => request.Methodology)
            .NotEmpty().WithMessage(Errors.General.MissingField("methodology").Message);
    }
}
