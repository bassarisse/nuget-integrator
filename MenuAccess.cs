using UnityEditor;

namespace Bassarisse.NuGetIntegrator
{
    public static class MenuAccess
    {
        [MenuItem("NuGet/Create Integration Project")]
        private static void CreateIntegrationProject() => CommandRunner.EnsureProject();

        [MenuItem("NuGet/Restore Dependencies")]
        private static async void RestoreDependencies() => await CommandRunner.Publish();
    }
}
