namespace Smart.FA.Catalog.UserAdmin.Web.Extensions;

public static class HostEnvironmentExtensions
{
    public const string LocalEnvironmentName = "Local";

    public static bool IsLocalEnvironment(this IHostEnvironment webHostEnvironment)
    {
        return webHostEnvironment.IsEnvironment(LocalEnvironmentName);
    }
}
