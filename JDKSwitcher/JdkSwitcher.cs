using System.Diagnostics;

public static class JdkSwitcher
{
    public static string SetJdk(string jdkPath)
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