using FluentValidation;
using Smart.FA.Catalog.UserAdmin.Domain.Exceptions;

namespace Smart.FA.Catalog.UserAdmin.Domain.Domain.Validators;

public class TrainingLocalizedDetailsValidator: AbstractValidator<TrainingLocalizedDetails>
{
    public TrainingLocalizedDetailsValidator()
    {
        RuleFor(request => request.Title)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("title").Message);
        RuleFor(request => request.Language)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("language").Message);
        RuleFor(request => request.Goal)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("goal").Message);
        RuleFor(request => request.Methodology)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("methodology").Message);
        RuleFor(request => request.PracticalModalities)
            .NotEmptyWithGenericMessage().WithMessage(Errors.General.MissingField("practicalModalities").Message);
    }
}
