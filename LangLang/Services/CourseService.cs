using LangLang.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangLang.Model;

namespace LangLang.Services
{
    internal class CourseService
    {
        CourseDAO courseDAO = CourseDAO.getInstance();

        public Dictionary<string,Course> GetAll()
        {
            return courseDAO.getAllCourses();
        }

        public void AddCourse(Course course)
        {
            courseDAO.AddCourse(course);
        }

        public Course GetCourseById(string id)
        {
            return courseDAO.GetCourseById(id);
        }

        public void DeleteCourse(string id)
        {
            courseDAO.DeleteCourse(id);
        }

        public void UpdateCourse(Course course)
        {
            courseDAO.UpdateCourse(course);
        }

    }
}
