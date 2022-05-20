using System.ComponentModel.DataAnnotations;
using Smart.FA.Catalog.Localization;
using Smart.FA.Catalog.Showcase.Localization;

namespace Smart.FA.Catalog.Showcase.Infrastructure.Mailing.Contact;

public class InquirySendEmailRequest
{
    [Required(ErrorMessageResourceType = typeof(ShowcaseResources), ErrorMessageResourceName = "FieldIsRequired")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessageResourceType = typeof(CatalogResources), ErrorMessageResourceName = "EmailIsRequired")]
    [EmailAddress(ErrorMessageResourceType = typeof(CatalogResources), ErrorMessageResourceName = "InvalidEmail")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessageResourceType = typeof(ShowcaseResources), ErrorMessageResourceName = "FieldIsRequired")]
    [MinLength(30, ErrorMessageResourceType = typeof(CatalogResources), ErrorMessageResourceName = "Min30Char")]
    [MaxLength(1000, ErrorMessageResourceType = typeof(CatalogResources), ErrorMessageResourceName = "Min30CharMax1000Char")]
    public string Message { get; set; } = null!;
}
