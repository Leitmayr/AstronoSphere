using System;
using System.Windows;
using Astronometria.ScientificRun.Hosting;

namespace Astronometria
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e.Args.Length > 0)
            {
                int exitCode;

                try
                {
                    exitCode = ScientificRunHost.Run(e.Args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("=== Astronometria ScientificRun failed ===");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex);
                    exitCode = 1;
                }

                Shutdown(exitCode);
                return;
            }

            var window = new MainWindow();
            MainWindow = window;
            window.Show();
        }
    }
}