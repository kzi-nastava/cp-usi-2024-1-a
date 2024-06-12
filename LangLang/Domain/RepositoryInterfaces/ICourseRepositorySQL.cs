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
        public Course? Update(string id, Course course);
        public void Delete(string id);
    }
}
