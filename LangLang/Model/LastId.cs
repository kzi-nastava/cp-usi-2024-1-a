
namespace LangLang.Model
{
    internal class LastId
    {
        public int CourseId { get; set; }
        public int ExamId { get; set; }
        public int CourseApplicationId { get; set; }
        public int CourseAttendanceId {  get; set; }

        public LastId()
        {
            CourseId = 0;
            ExamId = 0;
            CourseApplicationId = 0;
            CourseAttendanceId = 0;
        }
        public LastId(int courseId, int examId, int courseApplicationId, int courseAttendanceId)
        {
            CourseId = courseId;
            ExamId = examId;
            CourseApplicationId = courseApplicationId;
            CourseAttendanceId = courseAttendanceId;
        }
    }
}
