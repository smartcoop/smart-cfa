namespace Smart.FA.Catalog.UserAdmin.Web.Authentication.Header;

// The extra data passed in the header "customData" received by Nginx
public class AccountData
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}
