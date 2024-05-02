using LangLang.MVVM;
using LangLang.ViewModel;
using LangLang.ViewModel.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LangLang.HostBuilders;

public static class AddViewModelsHostBuilderExtensions
{
    public static IHostBuilder AddViewModels(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddSingleton<ILangLangViewModelFactory, LangLangViewModelFactory>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddScoped<StudentViewModel>();
            services.AddScoped<TutorViewModel>();
            services.AddScoped<DirectorViewModel>();
            services.AddScoped<CourseViewModel>();
            services.AddScoped<ExamViewModel>();
            services.AddTransient<StudentAccountViewModel>();
            services.AddTransient<NotificationViewModel>();
            services.AddTransient<ActiveCourseInfoViewModel>();
            services.AddTransient<UpcomingCourseInfoViewModel>();
            services.AddTransient<FinishedCourseInfoViewModel>();
            
            services.AddScoped<CreateViewModel<LoginViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<LoginViewModel>
            );
            
            services.AddScoped<CreateViewModel<RegisterViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<RegisterViewModel>
            );
            
            services.AddScoped<CreateViewModel<StudentViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<StudentViewModel>
            );
            
            services.AddScoped<CreateViewModel<TutorViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<TutorViewModel>
            );
            
            services.AddScoped<CreateViewModel<DirectorViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<DirectorViewModel>
            );
            
            services.AddScoped<CreateViewModel<CourseViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<CourseViewModel>
            );
            
            services.AddScoped<CreateViewModel<ExamViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<ExamViewModel>
            );
            
            services.AddScoped<CreateViewModel<StudentAccountViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<StudentAccountViewModel>
            );
            services.AddScoped<CreateViewModel<ActiveCourseInfoViewModel>>(
                servicesProvider => servicesProvider.GetRequiredService<ActiveCourseInfoViewModel>
            );
            services.AddScoped<CreateViewModel<UpcomingCourseInfoViewModel>>(
                servicesProvider => servicesProvider.GetRequiredService<UpcomingCourseInfoViewModel>
            );
            services.AddScoped<CreateViewModel<FinishedCourseInfoViewModel>>(
                servicesProvider => servicesProvider.GetRequiredService<FinishedCourseInfoViewModel>
            );
            
            services.AddScoped<CreateViewModel<NotificationViewModel>>(
                servicesProvider => servicesProvider.GetRequiredService<NotificationViewModel>
            );
        });
        
        return host;
    }
}