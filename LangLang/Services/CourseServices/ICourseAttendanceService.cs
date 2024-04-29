using LangLang.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Services.StudentCourseServices;
public interface ICourseAttendanceService
{
    public void RemoveAttendee(string studentId, string courseId);
    public void GradeStudent(string studentId, string CourseId);
    public void RateTutor();
    public List<CourseAttendance> GetFinishedCoursesStudent(string studentId);
}

