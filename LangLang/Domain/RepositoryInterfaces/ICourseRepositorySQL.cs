using LangLang.Core;
using LangLang.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Domain.RepositoryInterfaces
{
    public interface ICourseRepositorySQL
    {
        public void Add(Course course);
        public List<Course> GetAll();
        public Course Get(string id);
        public List<Course> Get(List<string> ids);
        public List<Course> GetByTutorId(string tutorId);
        public List<Course> GetCoursesByDate(DateOnly date);
        public List<Course> GetForTimePeriod(DateTime from, DateTime to);
        public List<Course> GetAllForPage(int pageNumber, int coursesPerPage);
        public List<Course> GetByTutorIdForPage(string tutorId, int pageNumber, int coursesPerPage);
        public void Delete(string id);
        public Course? Update(string id, Course course);
    }
}
