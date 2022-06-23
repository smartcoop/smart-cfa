using System.Text.Json;

namespace Smart.FA.Catalog.Web.Authentication.Header;

// The extra data passed in the header "customData" received by Nginx
public class AccountData
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public Impersonator? AdminBehindUser { get; set; }
}

/// <summary>
/// Data about a permanent member impersonating a trainer
/// </summary>
public class Impersonator
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? UserId { get; set; }
}
