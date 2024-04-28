using LangLang.Services.UserServices;

namespace LangLang.Services.EntityServices
{
    public class CourseApplicationService : ICourseApplicationService
    {   
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;

        public CourseApplicationService(ICourseService courseService, IStudentService studentService)
        {
            _courseService = courseService;
            _studentService = studentService;
        }


    }
}
