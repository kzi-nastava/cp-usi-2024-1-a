using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Model
{
    internal class LastId
    {
        public int CourseId { get; set; }
        public int ExamId { get; set; }

        public LastId()
        {
            CourseId = 0;
            ExamId = 0;
        }
        public LastId(int courseId, int examId)
        {
            CourseId = courseId;
            ExamId = examId;
        }
    }
}
