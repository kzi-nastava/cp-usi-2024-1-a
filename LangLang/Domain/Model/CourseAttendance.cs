namespace LangLang.Domain.Model
{
    public class CourseAttendance
    {
        public string Id {  get; set; }
        public string CourseId { get; set; }
        public string StudentId { get; set; }
        public int KnowledgeGrade {  get; set; }
        public int ActivityGrade { get; set; }

        public bool IsRated { get; set; }

        public bool IsGraded { get; set; }

        public CourseAttendance() {
            Id = "";
            CourseId = "";
            StudentId = "";
            IsRated = false;
            IsGraded = false;
            KnowledgeGrade = 0;
            ActivityGrade = 0;
        }

        public CourseAttendance(string courseId, string studentId, bool isRated, bool isGraded, int activityGrade, int knowledgeGrade)
        {
            Id = "";
            CourseId = courseId;
            StudentId = studentId;
            this.IsRated = isRated;
            this.IsGraded = isGraded;
            ActivityGrade = activityGrade;
            KnowledgeGrade = knowledgeGrade;
        }

        public CourseAttendance(string id, string courseId, string studentId, bool isRated, bool isGraded, int activityGrade, int knowledgeGrade)
        {
            Id = id;
            CourseId = courseId;
            StudentId = studentId;
            this.IsRated = isRated;
            this.IsGraded = isGraded;
            KnowledgeGrade = knowledgeGrade;
            ActivityGrade = activityGrade;
        }

        public void AddGrade(int activityGrade, int knowledgeGrade)
        {
            IsGraded = true; 
            KnowledgeGrade = knowledgeGrade;
            ActivityGrade = activityGrade;
        }

        public void AddRating()
        {
            IsRated = true;
        }

    }
}
