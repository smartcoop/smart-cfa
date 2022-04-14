using FluentValidation;
using Smart.FA.Catalog.Core.Helpers;

namespace Smart.FA.Catalog.Application.Extensions.FluentValidation;

public static class OnStringRuleBuilderExtensions
{
    /// <summary>
    /// Defines a rules to validate an email format.
    /// The rule checks if the email is null or empty, then validates the format of the email.
    /// </summary>
    /// <typeparam name="T">Type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The <see cref="IRuleBuilder{T,TProperty}" /> on which is applied the rule.</param>
    /// <returns>The source builder source, namely <paramref name="ruleBuilder" />.</returns>
    public static IRuleBuilder<T, string?> ValidEmail<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(email => EmailValidator.Validate(email))
            .WithMessage(CatalogResources.InvalidEmail);
    }
}
