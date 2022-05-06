using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace Smart.FA.Catalog.Shared.Security;

public static class NetSecurityNativeFix
{
    public static void Initialize(ILogger logger)
    {
        var result = -1;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            result = NetSecurityNative_EnsureGssInitialized();
            if (result is 0)
            {
                logger.LogInformation("{ServiceName}::{MethodName} EnsureGssInitialized workaround succeeded", nameof(NetSecurityNativeFix), nameof(Initialize));
            }
            else
            {
                logger.LogInformation("{ServiceName}::{MethodName} EnsureGssInitialized has failed result {Result}", nameof(NetSecurityNativeFix), nameof(Initialize), result);
            }
        }
    }

    [DllImport("System.Net.Security.Native")]
    [DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.UserDirectories)]
    private static extern int NetSecurityNative_EnsureGssInitialized();
}
