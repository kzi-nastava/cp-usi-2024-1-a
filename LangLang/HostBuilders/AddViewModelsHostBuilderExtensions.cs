using LangLang.WPF.MVVM;
using LangLang.WPF.ViewModels.Common;
using LangLang.WPF.ViewModels.Director;
using LangLang.WPF.ViewModels.Factories;
using LangLang.WPF.ViewModels.Student;
using LangLang.WPF.ViewModels.Tutor;
using LangLang.WPF.ViewModels.Tutor.Course;
using LangLang.WPF.ViewModels.Tutor.Exam;
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
            services.AddTransient<StudentMenuViewModel>();
            services.AddScoped<TutorMenuViewModel>();
            services.AddScoped<DirectorMenuViewModel>();
            services.AddScoped<CourseOverviewViewModel>();
            services.AddScoped<ExamOverviewViewModel>();
            services.AddTransient<StudentAccountViewModel>();
            services.AddTransient<NotificationListViewModel>();
            services.AddTransient<ActiveCourseViewModel>();
            services.AddTransient<UpcomingCourseViewModel>();
            services.AddTransient<FinishedCourseViewModel>();
            services.AddTransient<ActiveExamViewModel>();
            services.AddTransient<UpcomingExamViewModel>();
            services.AddTransient<FinishedExamViewModel>();

            services.AddTransient<TutorOverviewViewModel>();
            services.AddTransient<RateTutorViewModel>();
            
            services.AddScoped<CreateViewModel<LoginViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<LoginViewModel>
            );
            
            services.AddScoped<CreateViewModel<RegisterViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<RegisterViewModel>
            );
            
            services.AddScoped<CreateViewModel<StudentMenuViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<StudentMenuViewModel>
            );
            
            services.AddScoped<CreateViewModel<TutorMenuViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<TutorMenuViewModel>
            );
            
            services.AddScoped<CreateViewModel<DirectorMenuViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<DirectorMenuViewModel>
            );
            
            services.AddScoped<CreateViewModel<CourseOverviewViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<CourseOverviewViewModel>
            );
            
            services.AddScoped<CreateViewModel<ExamOverviewViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<ExamOverviewViewModel>
            );
            
            services.AddScoped<CreateViewModel<StudentAccountViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<StudentAccountViewModel>
            );
            services.AddScoped<CreateViewModel<ActiveCourseViewModel>>(
                servicesProvider => servicesProvider.GetRequiredService<ActiveCourseViewModel>
            );
            services.AddScoped<CreateViewModel<UpcomingCourseViewModel>>(
                servicesProvider => servicesProvider.GetRequiredService<UpcomingCourseViewModel>
            );
            services.AddScoped<CreateViewModel<FinishedCourseViewModel>>(
                servicesProvider => servicesProvider.GetRequiredService<FinishedCourseViewModel>
            );
            services.AddScoped<CreateViewModel<ActiveExamViewModel>>(
                servicesProvider => servicesProvider.GetRequiredService<ActiveExamViewModel>
            );
            services.AddScoped<CreateViewModel<UpcomingExamViewModel>>(
                servicesProvider => servicesProvider.GetRequiredService<UpcomingExamViewModel>
            );
            services.AddScoped<CreateViewModel<FinishedExamViewModel>>(
                servicesProvider => servicesProvider.GetRequiredService<FinishedExamViewModel>
            );


            services.AddScoped<CreateViewModel<TutorOverviewViewModel>>(
                servicesProvider => servicesProvider.GetRequiredService<TutorOverviewViewModel>
            );

            services.AddScoped<CreateViewModel<NotificationListViewModel>>(
                servicesProvider => servicesProvider.GetRequiredService<NotificationListViewModel>
            );

            services.AddScoped<CreateViewModel<RateTutorViewModel>>(
            servicesProvider => servicesProvider.GetRequiredService<RateTutorViewModel>
            );
        });
        
        return host;
    }
}