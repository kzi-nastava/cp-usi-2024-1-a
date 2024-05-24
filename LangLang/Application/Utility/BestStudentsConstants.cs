namespace LangLang.Application.Utility
{
    public class BestStudentsConstants
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
    }
}
