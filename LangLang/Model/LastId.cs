namespace LangLang.Model
{
    internal class LastId
    {
        public int CourseId { get; set; }
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public int TutorId { get; set; }

        public LastId()
        {
            CourseId = 0;
            ExamId = 0;
            StudentId = 0;
            TutorId = 0;
        }
        public LastId(int courseId, int examId, int studentId, int tutorId)
        {
            CourseId = courseId;
            ExamId = examId;
            StudentId = studentId;
            TutorId = tutorId;
        }
    }
}
