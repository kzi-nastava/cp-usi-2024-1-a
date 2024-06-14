using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using LangLang.CLI.Views;
using LangLang.HostBuilders;
using LangLang.WPF.Views.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LangLang
{
    public partial class App : System.Windows.Application
    {
        private readonly IHost _host;

        public App()
        {
            var args = Environment.GetCommandLineArgs();
            _host = args.Contains("--cli") ? CreateCliHostBuilder().Build() : CreateHostBuilder().Build();
        }

        private static IHostBuilder CreateHostBuilder(string[]? args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, builder) => builder.AddUserSecrets<App>())
                .AddRepositories()
                .AddServices()
                .AddStores()
                .AddViewModels()
                .AddWindows();
        }

        private static IHostBuilder CreateCliHostBuilder(string[]? args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => logging.ClearProviders())
                .ConfigureAppConfiguration((_, builder) => builder.AddUserSecrets<App>())
                .AddRepositories()
                .AddServices()
                .AddStores()
                .AddMenus();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            
            if (e.Args.Contains("--cli"))
            {
                try
                {
                    const int attachParentProcess = -1;
                    if (AttachConsole(attachParentProcess))
                    {
                        ICliMenu loginMenu = _host.Services.GetRequiredService<LoginMenu>();
                        loginMenu.Show();
                    }
                    // if attach to parent process fails alloc new console
                    else
                    {
                        AllocConsole();
                        ICliMenu loginMenu = _host.Services.GetRequiredService<LoginMenu>();
                        loginMenu.Show();

                    }
                }
                finally
                {
                    FreeConsole();
                    Shutdown();
                }
            }
            else
            {
                Window window = _host.Services.GetRequiredService<LoginWindow>();
                window.Show();
            }
            
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.StopAsync();
            base.OnExit(e);
        }
        
        [DllImport("kernel32")]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        [DllImport("kernel32")]
        private static extern bool FreeConsole();

    }
}
