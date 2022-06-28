namespace Smart.FA.Catalog.Application.SeedWork.Attributes;

/// <summary>
/// Specifies that a property should be sanitized.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SanitizedAttribute : Attribute
{
}
