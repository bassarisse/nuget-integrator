using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Bassarisse.NuGetIntegrator.Utils
{
    public static class ProcessExtensions
    {
        public static Task RunAsync(this Process process)
        {
            var tcs = new TaskCompletionSource<object>();
            process.EnableRaisingEvents = true;

            void OnProcessExited(object s, EventArgs e) => tcs.TrySetResult(null);
            process.Exited += OnProcessExited;

            // not sure on best way to handle false being returned
            if (!process.Start()) tcs.SetException(new Exception("Failed to start process."));
            return tcs.Task;
        }
    }
}