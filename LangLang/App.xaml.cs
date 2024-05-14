using System.Windows;
using LangLang.HostBuilders;
using LangLang.WPF.Views.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LangLang
{
    public partial class App : System.Windows.Application
    {
        private readonly IHost _host = CreateHostBuilder().Build();

        private static IHostBuilder CreateHostBuilder(string[]? args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .AddDao()
                .AddServices()
                .AddStores()
                .AddViewModels()
                .AddWindows();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            Window window = _host.Services.GetRequiredService<LoginWindow>();
            window.Show();
            
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.StopAsync();
            base.OnExit(e);
        }
    }
}
