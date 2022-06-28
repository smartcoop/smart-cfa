using FluentValidation;
using HtmlAgilityPack;
using Smart.FA.Catalog.Shared.Collections;

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

    /// <summary>
    /// Defines a length validator on the current rule builder, but only for string properties that are HTML.
    /// Validation will fail if the length of the HTML inner text is less than the length specified.
    /// </summary>
    /// <typeparam name="T">Type of object being validated</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
    /// <param name="minimumLength"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> MinimumHtmlInnerLength<T>(this IRuleBuilder<T, string?> ruleBuilder, int minimumLength)
    {
        return ruleBuilder.Must(html =>
        {
            if (html is null)
            {
                return true;
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            return htmlDocument.DocumentNode.InnerText.Length >= minimumLength;
        });
    }

    /// <summary>
    /// Defines a length validator on the current rule builder, but only for string properties that are HTML.
    /// Validation will fail if the length of the HTML's inner text is larger than the length specified.
    /// </summary>
    /// <typeparam name="T">Type of object being validated</typeparam>
    /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
    /// <param name="maximumLength"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> MaximumHtmlInnerLength<T>(this IRuleBuilder<T, string?> ruleBuilder, int maximumLength)
    {
        return ruleBuilder.Must(html =>
        {
            if (html is null)
            {
                return true;
            }

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            return htmlDocument.DocumentNode.InnerText.Length <= maximumLength;
        });
    }
}
