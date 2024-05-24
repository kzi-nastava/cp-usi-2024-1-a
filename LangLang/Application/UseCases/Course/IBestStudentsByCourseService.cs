using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Application.UseCases.Course;

public interface IBestStudentsByCourseService
{
    public void SendEmailToBestStudents(string courseId, Domain.Model.GradingPriority.Priority priority);

}
