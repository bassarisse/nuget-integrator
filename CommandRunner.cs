using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Bassarisse.NuGetIntegrator.Utils;
using Debug = UnityEngine.Debug;

namespace Bassarisse.NuGetIntegrator
{
    public static class CommandRunner
    {
        public const string PackageName = "com.bassarisse.nuget-integrator";
        public const string IntegrationPath = "NuGetIntegration/integration.csproj";

        public static void EnsureProject()
        {
            if (File.Exists(IntegrationPath))
                return;

            var projPath = Path.GetDirectoryName(IntegrationPath) ?? "./";

            var templateRoot = Path.Combine(
                Path.GetDirectoryName(Path.GetFullPath($"Packages/{PackageName}/package.json")) ?? "",
                "Template~"
            );

            FileUtils.EnsureDirectory(projPath);
            FileUtils.CopyFilesRecursively(new DirectoryInfo(templateRoot), new DirectoryInfo(projPath));
        }

        // dotnet publish NuGetIntegration/integration.csproj -c Release
        public static async Task Publish()
        {
            var startTime = DateTime.UtcNow;

            EnsureProject();

            var script = new Process
            {
                StartInfo = {
                    WorkingDirectory = Environment.CurrentDirectory,
#if UNITY_EDITOR_OSX
                    FileName = "/usr/local/share/dotnet/dotnet",
#else
                    FileName = "dotnet",
#endif
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Normal,
                    Arguments = $"publish {IntegrationPath} -c Release",
                }
            };

            await script.RunAsync();

            var output = script.StandardOutput.ReadToEnd();
            if (!string.IsNullOrEmpty(output))
            {
                if (script.ExitCode != 0)
                    Debug.LogError(output.Trim());
                else
                    Debug.Log(output.Trim());
            }

            var errorOutput = script.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(errorOutput))
                Debug.LogError(errorOutput.Trim());

            var elapsedTime = DateTime.UtcNow - startTime;
            var formattedTime = $"{elapsedTime.TotalMilliseconds / 1000f:0.00}s";

            if (script.ExitCode != 0)
                throw new Exception($"Error acquiring NuGet dependencies! {formattedTime}");

            Debug.Log($"Acquired NuGet dependencies! {formattedTime}");
        }
    }
}