
namespace LangLang.Domain.Model
{
    internal class LastId
    {
        public int CourseId { get; set; }
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public int TutorId { get; set; }
        public int CourseApplicationId { get; set; }
        public int CourseAttendanceId { get; set; }
        public int NotificationId { get; set; }
        public int ExamApplicationId { get; set; }
        public int ExamAttendanceId { get; set; }
        public int DropRequestId { get; set; }

        public LastId()
        {
            CourseId = 0;
            ExamId = 0;
            StudentId = 0;
            TutorId = 0;
            CourseApplicationId = 0;
            CourseAttendanceId = 0;
            NotificationId = 0;
            DropRequestId = 0;
            ExamApplicationId = 0;
            ExamAttendanceId = 0;
        }

        public LastId(int courseId, int examId, int studentId, int tutorId, int courseApplicationId, int courseAttendanceId, int notificationId, int dropRequestId, int examApplicationId, int examAttendanceId)
        {
            CourseId = courseId;
            ExamId = examId;
            StudentId = studentId;
            TutorId = tutorId;
            CourseApplicationId = courseApplicationId;
            CourseAttendanceId = courseAttendanceId;
            NotificationId = notificationId;
            DropRequestId = dropRequestId;
            ExamApplicationId = examApplicationId;
            ExamAttendanceId = examAttendanceId;
        }
    }
}
