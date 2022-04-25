using System.Runtime.InteropServices;

namespace Smart.FA.Catalog.Web.Security;

public static class NetSecurityNativeFix {
    public static void Initialize() {
        var result = -1;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
            result = NetSecurityNative_EnsureGssInitialized();
        }
        if (result is 0) {
            Console.WriteLine($@"{nameof(NetSecurityNativeFix)}::{nameof(Initialize)} EnsureGssInitialized workaround succeeded");
        } else {
            Console.WriteLine($@"{nameof(NetSecurityNativeFix)}::{nameof(Initialize)} EnsureGssInitialized has failed result {result}");
        }
    }

    [DllImport("System.Net.Security.Native")]
    [DefaultDllImportSearchPaths(System.Runtime.InteropServices.DllImportSearchPath.UserDirectories)]
    private static extern int NetSecurityNative_EnsureGssInitialized();
}
