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

        public bool isGrated;

        public CourseAttendance() {
            Id = "";
            CourseId = "";
            StudentId = "";
            isRated = false;
            isGrated = false;
        }

        public CourseAttendance(string courseId, string studentId, bool isRated, bool isGrated)
        {
            Id = "";
            CourseId = courseId;
            StudentId = studentId;
            this.isRated = isRated;
            this.isGrated = isGrated;
        }

        public CourseAttendance(string id, string courseId, string studentId, bool isRated, bool isGrated)
        {
            Id = id;
            CourseId = courseId;
            StudentId = studentId;
            this.isRated = isRated;
            this.isGrated = isGrated;
        }

        public void AddGrade()
        {
            isGrated = true;
        }

        public void AddRating()
        {
            isRated = true;
        }

    }
}
