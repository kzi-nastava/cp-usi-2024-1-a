using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Model
{
    public class CourseAttendance
    {
        public string Id {  get; set; }
        public string CourseId { get; set; }
        public string StudentId { get; set; }
        public int KnowledgeGrade {  get; set; }
        public int ActivityGrade { get; set; }

        public bool isRated;

        public bool isGraded;

        public CourseAttendance() {
            Id = "";
            CourseId = "";
            StudentId = "";
            isRated = false;
            isGraded = false;
            KnowledgeGrade = 0;
            ActivityGrade = 0;
        }

        public CourseAttendance(string courseId, string studentId, bool isRated, bool isGraded, int activityGrade, int knowledgeGrade)
        {
            Id = "";
            CourseId = courseId;
            StudentId = studentId;
            this.isRated = isRated;
            this.isGraded = isGraded;
            ActivityGrade = activityGrade;
            KnowledgeGrade = knowledgeGrade;
        }

        public CourseAttendance(string id, string courseId, string studentId, bool isRated, bool isGraded, int activityGrade, int knowledgeGrade)
        {
            Id = id;
            CourseId = courseId;
            StudentId = studentId;
            this.isRated = isRated;
            this.isGraded = isGraded;
            KnowledgeGrade = knowledgeGrade;
            ActivityGrade = activityGrade;
        }

        public void AddGrade(int activityGrade, int knowledgeGrade)
        {
            isGraded = true; 
            KnowledgeGrade = knowledgeGrade;
            ActivityGrade = activityGrade;
        }

        public void AddRating()
        {
            isRated = true;
        }

    }
}
