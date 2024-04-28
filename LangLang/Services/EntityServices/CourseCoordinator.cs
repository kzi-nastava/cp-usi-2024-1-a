using LangLang.Services.UserServices;

namespace LangLang.Services.EntityServices
{
    public class CourseCoordinator : ICourseCoordinator
    {
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;
        private readonly ICourseApplicationService _courseApplicationService;

        public CourseCoordinator(ICourseService courseService, IStudentService studentService, ICourseApplicationService courseApplicationService)
        {
            _courseService = courseService;
            _studentService = studentService;
            _courseApplicationService = courseApplicationService;
        }







    }
}
