using LangLang.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LangLang.HostBuilders;

public static class AddStoresHostBuilderExtensions
{
    public static IHostBuilder AddStores(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<IAuthenticationStore, AuthenticationStore>();
            services.AddSingleton<CurrentCourseStore>();
        });
        
        return host;
    }
}