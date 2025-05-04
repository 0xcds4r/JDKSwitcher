using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

public static class JdkFinder
{
    public static List<string> FindJdks(string searchPath = "/usr/lib/jvm")
    {
        var jdks = new List<string>();
        if (Directory.Exists(searchPath))
        {
            foreach (var dir in Directory.GetDirectories(searchPath))
            {
                if (File.Exists(Path.Combine(dir, "bin", "java")))
                    jdks.Add(dir);
            }
        }
        return jdks;
    }

    public static string? GetCurrentJdk()
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