namespace LangLang.Model
{
    internal class CourseApplication
    {
        public string AppliedCourseId {  get; set; }
        public string ApplicantId {  get; set; }

        public CourseApplication() {
            AppliedCourseId = "-1";
            ApplicantId = "";
        }

        public CourseApplication(string appliedCourseId, string applicantId)
        {
            AppliedCourseId = appliedCourseId;
            ApplicantId = applicantId;
        }


    }
}
