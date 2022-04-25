using System.Runtime.InteropServices;
using Smart.FA.Catalog.Infrastructure.Services;

namespace Smart.FA.Catalog.Web.Security;

public static class NetSecurityNativeFix
{
    public static void Initialize(ILogger<BootStrapService> logger)
    {
        var result = -1;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            result = NetSecurityNative_EnsureGssInitialized();
        }

        if (result is 0)
        {
            logger.LogInformation("{ServiceName}::{MethodName} EnsureGssInitialized workaround succeeded", nameof(NetSecurityNativeFix), nameof(Initialize));
        }
        else
        {
            logger.LogInformation("{ServiceName}::{MethodName} EnsureGssInitialized has failed result {Result}", nameof(NetSecurityNativeFix), nameof(Initialize), result);
        }
    }

    [DllImport("System.Net.Security.Native")]
    [DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.UserDirectories)]
    private static extern int NetSecurityNative_EnsureGssInitialized();
}
