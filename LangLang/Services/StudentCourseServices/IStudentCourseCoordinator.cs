using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Services.StudentCourseServices;

public interface IStudentCoureCoordinator
{
    public void Accept(string applicationId);
    public void CancelAplication(string  applicationId);
    public void RemoveAttendee(string  courseId, string  studentId);
    public void ApplyForCourse(string  courseId, string  studentId);
    public void FinishCourse(string courseId, string studentId);
    public void GenerateAttendance();
    public void DropCourse(string  courseId);

}
