using LangLang.Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Services.EntityServices
{
    public class CourseCoordinator : ICourseCoordinator
    {
        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;

        public CourseCoordinator(ICourseService courseService, IStudentService studentService)
        {
            _courseService = courseService;
            _studentService = studentService;
        }







    }
}
