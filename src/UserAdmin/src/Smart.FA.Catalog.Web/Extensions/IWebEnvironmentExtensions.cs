namespace Microsoft.Extensions.Hosting;

public static class HostEnvironmentExtensions
{
    public const string LocalEnvironmentName = "Local";

    public static bool IsLocalEnvironment(this IWebHostEnvironment webHostEnvironment)
    {
        return webHostEnvironment.IsEnvironment(LocalEnvironmentName);
    }
}
