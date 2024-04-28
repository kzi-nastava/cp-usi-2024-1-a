using LangLang.Model;
using LangLang.Services.AuthenticationServices;
using LangLang.Services.EntityServices;
using LangLang.Services.NavigationServices;
using LangLang.Services.UserServices;
using LangLang.Services.UtilityServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LangLang.HostBuilders;

public static class AddServicesHostBuilderExtensions
{
    public static IHostBuilder AddServices(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
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
            services.AddSingleton<IPopupNavigationService, PopupNavigationService>();
            services.AddSingleton<IClosePopupNavigationService, ClosePopupNavigationService>();
            services.AddSingleton<ICourseCoordinator, CourseCoordinator>();
            services.AddSingleton<ICourseApplicationService, CourseApplicationService>();
            services.AddSingleton<ICourseAttendanceService, CourseAttendanceService>();
        });
        
        return host;
    }
}