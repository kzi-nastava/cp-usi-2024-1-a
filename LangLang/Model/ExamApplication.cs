namespace LangLang.Model;

public class ExamApplication
{
    public string Id { get; set; }
    public string ExamId { get; set; }
    public string StudentId { get; set; }
    public State ExamApplicationState { get; set; }
    
    public enum State
    {
        Pending, Rejected, Accepted
    }

    public ExamApplication()
    {
        Id = "";
        ExamId = "";
        StudentId = "";
    }

    public ExamApplication(string examId, string studentId)
    {
        Id = "";
        ExamId = examId;
        StudentId = studentId;
        ExamApplicationState = State.Pending;
    }

    public ExamApplication(string id, string examId, string studentId, State examApplicationState)
    {
        Id = id;
        ExamId = examId;
        StudentId = studentId;
        ExamApplicationState = examApplicationState;
    }
}