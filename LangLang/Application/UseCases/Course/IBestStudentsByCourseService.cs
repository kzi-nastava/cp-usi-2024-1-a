namespace LangLang.Application.UseCases.Course;

public interface IBestStudentsByCourseService
{
    public void SendEmailToBestStudents(string courseId, Utility.BestStudentsConstants.GradingPriority priority);

}
