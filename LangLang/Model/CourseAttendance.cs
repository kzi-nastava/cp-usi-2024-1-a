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

        public bool isRated;

        public bool isGraded;

        public CourseAttendance() {
            Id = "";
            CourseId = "";
            StudentId = "";
            isRated = false;
            isGraded = false;
        }

        public CourseAttendance(string courseId, string studentId, bool isRated, bool isGraded)
        {
            Id = "";
            CourseId = courseId;
            StudentId = studentId;
            this.isRated = isRated;
            this.isGraded = isGraded;
        }

        public CourseAttendance(string id, string courseId, string studentId, bool isRated, bool isGraded)
        {
            Id = id;
            CourseId = courseId;
            StudentId = studentId;
            this.isRated = isRated;
            this.isGraded = isGraded;
        }

        public void AddGrade()
        {
            isGraded = true;
        }

        public void AddRating()
        {
            isRated = true;
        }

    }
}
