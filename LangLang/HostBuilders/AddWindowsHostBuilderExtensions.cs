using LangLang.View;
using LangLang.View.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LangLang.HostBuilders;

public static class AddWindowsHostBuilderExtensions
{
    public static IHostBuilder AddWindows(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddSingleton<ILangLangWindowFactory, LangLangWindowFactory>();
            services.AddTransient<LoginWindow>();
        });
        
        return host;
    }
}