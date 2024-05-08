﻿using Microsoft.Windows.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LangLang.Model
{
    public class ExamAttendance
    {
        public string Id {  get; set; }
        public string ExamId { get; set; }
        public string StudentId { get; set; }
        public ExamGrade? Grade { get; set; }

        public bool isRated;

        [JsonIgnore]
        public bool IsGraded => Grade != null;

        public ExamAttendance() {
            Id = "";
            ExamId = "";
            StudentId = "";
            isRated = false;
            Grade = null;
        }

        public ExamAttendance(string examId, string studentId, bool isRated, bool isGraded, ExamGrade? grade = null)
        {
            Id = "";
            ExamId = examId;
            StudentId = studentId;
            this.isRated = isRated;
            if (grade != null)
                Grade = grade;
        }

        public void AddRating()
        {
            isRated = true;
        }
    }
}
