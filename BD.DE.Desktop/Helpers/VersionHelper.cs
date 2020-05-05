using System;
using System.Diagnostics;

namespace BD.DE.Desktop.Helpers
{
    public static class VersionHelper
    {
        public static string GetCurrentAppVersion()
        {
            var versionString = "unknown";
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var version = assembly.GetName().Version;
                versionString = version.ToString();
            }
            catch (Exception x)
            {
                Debug.WriteLine(x.Message);
            }
            return versionString;
        }
    }
}
