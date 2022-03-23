using FluentValidation;
using Smart.FA.Catalog.Core.Exceptions;

namespace Smart.FA.Catalog.Core.Domain.Validators;

public class TrainingDetailValidation: AbstractValidator<TrainingDetail>
{
    public TrainingDetailValidation()
    {
        RuleFor(request => request.Title)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("title").Message);
        RuleFor(request => request.Language)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("language").Message);
        RuleFor(request => request.Goal)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("goal").Message);
        RuleFor(request => request.Methodology)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("methodology").Message);
    }
}
