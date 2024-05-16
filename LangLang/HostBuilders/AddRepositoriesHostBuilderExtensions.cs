using LangLang.Domain.RepositoryInterfaces;
using LangLang.Repositories;
using LangLang.Repositories.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LangLang.HostBuilders;

public static class AddRepositoriesHostBuilderExtensions
{
    public static IHostBuilder AddRepositories(this IHostBuilder host)
    {
        host.ConfigureServices(services =>
        {
            services.AddSingleton<ICourseRepository, CourseRepository>(_ =>
                new CourseRepository(Constants.CourseFilePath, Constants.CourseIdFilePath));
            services.AddSingleton<IDirectorRepository, DirectorRepository>(_ =>
                new DirectorRepository(Constants.DirectorFilePath, Constants.DirectorIdFilePath));
            services.AddSingleton<IExamRepository, ExamRepository>(_ =>
                new ExamRepository(Constants.ExamFilePath, Constants.ExamIdFilePath));
            services.AddSingleton<ILanguageRepository, LanguageRepository>(_ =>
                new LanguageRepository(Constants.LanguageFilePath));
            services.AddSingleton<IStudentRepository, StudentRepository>(_ =>
                new StudentRepository(Constants.StudentFilePath, Constants.StudentIdFilePath));
            services.AddSingleton<ITutorRepository, TutorRepository>(_ =>
                new TutorRepository(Constants.TutorFilePath, Constants.TutorIdFilePath));
            services.AddSingleton<ICourseApplicationRepository, CourseApplicationRepository>(_ =>
                new CourseApplicationRepository(Constants.CourseApplicationsFilePath,
                    Constants.CourseApplicationsIdFilePath));
            services.AddSingleton<ICourseAttendanceRepository, CourseAttendanceRepository>(_ =>
                new CourseAttendanceRepository(Constants.CourseAttendancesFilePath,
                    Constants.CourseAttendancesIdFilePath));
            services.AddSingleton<IProfileRepository, ProfileRepository>(_ =>
                new ProfileRepository(Constants.ProfileFilePath));
            services.AddSingleton<IPersonProfileMappingRepository, PersonProfileMappingRepository>(_ =>
                new PersonProfileMappingRepository(Constants.PersonProfileMappingFilePath));
            services.AddSingleton<INotificationRepository, NotificationRepository>(_ =>
                new NotificationRepository(Constants.NotificationFilePath, Constants.NotificationIdFilePath));
            services.AddSingleton<IExamApplicationRepository, ExamApplicationRepository>(_ =>
                new ExamApplicationRepository(Constants.ExamApplicationFilePath, Constants.ExamApplicationIdFilePath));
            services.AddSingleton<IExamAttendanceRepository, ExamAttendanceRepository>(_ =>
                new ExamAttendanceRepository(Constants.ExamAttendancesFilePath, Constants.ExamAttendancesIdFilePath));
            services.AddSingleton<IDropRequestRepository, DropRequestRepository>(_ =>
                new DropRequestRepository(Constants.DropRequestFilePath, Constants.DropRequestIdFilePath));
        });

        return host;
    }
}