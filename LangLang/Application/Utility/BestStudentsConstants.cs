namespace LangLang.Application.Utility
{
    public static class BestStudentsConstants
    {
        public enum GradingPriority
        {
            KnowledgeGrade,
            ActivityGrade,
            EqualPriority
        }


        public const uint NumOfBestStudents = 3;
        public const int PenaltyPointWeight = -2;
        public const int ActivityGradeWeight = 2;
        public const int KnowledgeGradeWeight = 2;

        public const int PriorityFactor = 2;



        public const string bestStudentsAppreciationEmailBody = @"
        Dear {0},

        I hope this email finds you well.

        I am writing to extend my heartfelt congratulations to you for your exceptional performance in our {1}. Your dedication, knowledge, and active participation have truly set you apart from your peers. It has been a pleasure to witness your growth and commitment throughout the course.

        Your impressive achievements are a testament to your hard work and intellectual curiosity. You have consistently demonstrated a deep understanding of the material and have actively contributed to class discussions, enriching the learning experience for everyone.

        As one of the top students in this course, you have exemplified the qualities of an outstanding scholar. I have no doubt that you will continue to excel in your academic and professional endeavors.

        Thank you for your dedication and enthusiasm. It has been an honor to have you in the class, and I look forward to seeing all the great things you will accomplish in the future.

        Warm regards,

        LangLang school
    ";
    }
}
