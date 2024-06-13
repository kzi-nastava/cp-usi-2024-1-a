using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LangLang.Repositories.SQL
{
    public class CourseRepositorySQL : ICourseRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CourseRepositorySQL(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Course Add(Course course)
        {
            _dbContext.Courses.Add(course);
            _dbContext.SaveChanges();
            return course;
        }
    }
}
