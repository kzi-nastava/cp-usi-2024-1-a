using LangLang.DAO;
using LangLang.DAO.JsonDao;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LangLang.HostBuilders;

public static class AddDaoHostBuilderExtensions
{
    public static IHostBuilder AddDao(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddSingleton<ICourseDAO, CourseDAO>();
            services.AddSingleton<IDirectorDAO, DirectorDAO>();
            services.AddSingleton<IExamDAO, ExamDAO>();
            services.AddSingleton<ILanguageDAO, LanguageDAO>();
            services.AddSingleton<ILastIdDAO, LastIdDAO>();
            services.AddSingleton<IStudentDAO, StudentDAO>();
            services.AddSingleton<ITutorDAO, TutorDAO>();
            services.AddSingleton<ICourseApplicationDAO, CourseApplicationDAO>();
            services.AddSingleton<ICourseAttendanceDAO, CourseAttendanceDAO>();
        });
        
        return host;
    }
}