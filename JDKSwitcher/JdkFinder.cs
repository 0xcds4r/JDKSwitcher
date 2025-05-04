using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Linq;

public static class JdkFinder
{
    public static List<string> FindJdks(string? searchPath = null)
    {
        var jdks = new List<string>();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            var possibleDirs = new[]
            {
                Environment.GetEnvironmentVariable("JAVA_HOME", EnvironmentVariableTarget.User),
                Environment.GetEnvironmentVariable("JAVA_HOME", EnvironmentVariableTarget.Machine),
                @"C:\\Program Files\\Java",
                @"C:\\Program Files (x86)\\Java"
            };
            foreach (var baseDir in possibleDirs.Where(d => !string.IsNullOrWhiteSpace(d)))
            {
                if (Directory.Exists(baseDir))
                {
                    foreach (var dir in Directory.GetDirectories(baseDir))
                    {
                        if (File.Exists(Path.Combine(dir, "bin", "java.exe")))
                            jdks.Add(dir);
                    }
                    if (File.Exists(Path.Combine(baseDir, "bin", "java.exe")) && !jdks.Contains(baseDir))
                        jdks.Add(baseDir);
                }
            }
        }
        else
        {
            string path = searchPath ?? "/usr/lib/jvm";
            if (Directory.Exists(path))
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    if (File.Exists(Path.Combine(dir, "bin", "java")))
                        jdks.Add(dir);
                }
            }
        }
        return jdks.Distinct().ToList();
    }

    public static string? GetCurrentJdk()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Environment.GetEnvironmentVariable("JAVA_HOME", EnvironmentVariableTarget.User);
        }
        else
        {
            var psi = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = "-c \"update-alternatives --query java | grep ^Value: | cut -d' ' -f2\"",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            using var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd().Trim();
            process.WaitForExit();
            if (string.IsNullOrWhiteSpace(output)) return null;
            var dir = Path.GetDirectoryName(Path.GetDirectoryName(output));
            return dir;
        }
    }
} 