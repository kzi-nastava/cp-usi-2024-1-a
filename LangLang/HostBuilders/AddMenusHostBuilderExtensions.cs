using LangLang.CLI.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LangLang.HostBuilders;

public static class AddMenusHostBuilderExtensions
{
    public static IHostBuilder AddMenus(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddTransient<LoginMenu>();
            services.AddTransient<TutorMenu>();
            services.AddTransient<ExamMenu>();
            services.AddTransient<CourseMenu>();
        });
        
        return host;
    }
}