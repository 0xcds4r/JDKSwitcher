using System.Diagnostics;
using System;
using System.Runtime.InteropServices;
using System.Linq;
using System.IO;

public static class JdkSwitcher
{
    public static string SetJdk(string jdkPath)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Environment.SetEnvironmentVariable("JAVA_HOME", jdkPath, EnvironmentVariableTarget.User);
            var path = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User) ?? "";
            var javaBin = Path.Combine(jdkPath, "bin");
            var pathParts = path.Split(';').Select(p => p.Trim()).ToList();
            if (!pathParts.Any(p => string.Equals(p, javaBin, StringComparison.OrdinalIgnoreCase)))
            {
                path = javaBin + ";" + path;
                Environment.SetEnvironmentVariable("Path", path, EnvironmentVariableTarget.User);
                return $"JAVA_HOME установлен в {jdkPath}\nPath обновлён.";
            }
            return $"JAVA_HOME установлен в {jdkPath}\nPath уже содержит {javaBin}.";
        }
        else
        {
            var psi = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = $"-c \"sudo update-alternatives --set java {jdkPath}/bin/java\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            using var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            return string.IsNullOrWhiteSpace(error) ? output : output + "\n" + error;
        }
    }
} 