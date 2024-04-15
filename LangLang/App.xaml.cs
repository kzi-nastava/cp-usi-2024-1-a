using System;
using System.Windows;
using LangLang.MVVM;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.EntityServices;
using LangLang.Services.UserServices;
using LangLang.Services.UtilityServices;
using LangLang.Stores;
using LangLang.View;
using LangLang.View.Factories;
using LangLang.ViewModel;
using LangLang.ViewModel.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace LangLang
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceProvider serviceProvider = CreateServiceProvider();
            
            Window window = new LoginWindow(
                serviceProvider.GetRequiredService<LoginViewModel>(),
                serviceProvider.GetRequiredService<ILangLangWindowFactory>()
                );
            window.Show();
            
            base.OnStartup(e);
        }

        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            // Services
            services.AddSingleton<ILoginService, LoginService>();
            services.AddSingleton<IRegisterService, RegisterService>();
            services.AddSingleton<IStudentService, StudentService>();
            services.AddSingleton<ICourseService, CourseService>();
            services.AddSingleton<IExamService, ExamService>();
            services.AddSingleton<ITutorService, TutorService>();
            services.AddSingleton<IDirectorService, DirectorService>();
            services.AddSingleton<ILanguageService, LanguageService>();
            services.AddSingleton<ITimetableService, TimetableService>();
            services.AddSingleton<INavigationService, NavigationService>();
            
            // Stores
            services.AddSingleton<NavigationStore>();
            services.AddSingleton<AuthenticationStore>();
            
            // ViewModels
            services.AddSingleton<ILangLangViewModelFactory, LangLangViewModelFactory>();
            services.AddScoped<LoginViewModel>();
            services.AddScoped<RegisterViewModel>();
            services.AddScoped<StudentViewModel>();
            services.AddScoped<TutorViewModel>();
            services.AddScoped<DirectorViewModel>();
            services.AddScoped<CourseViewModel>();
            services.AddScoped<ExamViewModel>();
            
            services.AddScoped<CreateViewModel<LoginViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<LoginViewModel>
            );
            
            services.AddScoped<CreateViewModel<RegisterViewModel>>(
                serviceProvider => serviceProvider.GetRequiredService<RegisterViewModel>);
            
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
            
            // Window factory
            services.AddSingleton<ILangLangWindowFactory, LangLangWindowFactory>();

            return services.BuildServiceProvider();
        }
    }
}
