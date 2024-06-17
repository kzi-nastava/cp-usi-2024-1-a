using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Domain.Utility;
using Microsoft.EntityFrameworkCore;

namespace LangLang.Repositories.SQL
{
    public class CourseRepositorySQL : ICourseRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CourseRepositorySQL(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Course> GetCoursesByDate(DateOnly date)
        {
            var courses = _dbContext.Courses.Include(c => c.Language).ToList();

            var filteredCourses = courses
                .Where(course =>
                    date >= DateOnly.FromDateTime(course.Start) &&
                    date <= DateOnly.FromDateTime(course.Start.Add(TimeSpan.FromDays(7 * course.Duration))) &&
                    date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday &&
                    course.Schedule.ContainsKey(DayConverter.ToWorkDay(date.DayOfWeek))
                )
                .ToList();

            return filteredCourses;
        }


        public List<Course> GetForTimePeriod(DateTime from, DateTime to)
        {
            return _dbContext.Courses
                .Include(c => c.Language)
                .Where(course => course.Start >= from && course.Start <= to)
                .ToList();
        }

        public List<Course> GetByTutorId(string tutorId)
        {
            return _dbContext.Courses
                .Include(exam => exam.Language)
                .Where(course => course.TutorId == tutorId)
                .ToList();
        }

        public List<Course> GetAllForPage(int pageNumber, int coursesPerPage)
        {
            return _dbContext.Courses
                .Include(c => c.Language)
                .Skip((pageNumber - 1) * coursesPerPage)
                .Take(coursesPerPage)
                .ToList();
        }

        public List<Course> GetByTutorIdForPage(string tutorId, int pageNumber, int coursesPerPage)
        {
            return _dbContext.Courses
                .Include(c => c.Language)
                .Where(course => course.TutorId == tutorId)
                .Skip((pageNumber - 1) * coursesPerPage)
                .Take(coursesPerPage)
                .ToList();
        }

        public List<Course> GetAll()
        {
            return _dbContext.Courses.Include(c => c.Language).ToList();
        }

        public Course? Get(string id)
        {
            return _dbContext.Courses.Include(c => c.Language).FirstOrDefault(c => c.Id == id);
        }

        public List<Course> Get(List<string> ids)
        {
            return _dbContext.Courses.Include(c => c.Language).Where(course => ids.Contains(course.Id)).ToList();
        }

        public string GetId()
        {
            var lastCourse = _dbContext.Courses.OrderByDescending(c => c.Id).FirstOrDefault();
            int nextId = (lastCourse != null) ? int.Parse(lastCourse.Id) + 1 : 1;
            return nextId.ToString();
        }

        public Course Add(Course course)
        {
            var existingCourse = _dbContext.Courses.Include(c => c.Language).FirstOrDefault(c => c.Id == course.Id);

            if (existingCourse != null && course.Id != "-1")
            {
                _dbContext.Entry(existingCourse).CurrentValues.SetValues(course);
                _dbContext.SaveChanges();
            }
            else
            {
                course.Id = GetId();
                _dbContext.Courses.Add(course);
                _dbContext.SaveChanges();
            }
            return course;
        }

        public Course? Update(string id, Course course)
        {
            var existingCourse = _dbContext.Courses.Include(c => c.Language).FirstOrDefault(c => c.Id == course.Id);
            var language = _dbContext.Languages.Find(course.Language.Name);
            if (existingCourse == null)
                return null;

            if (language != null)
            {
                course.Language = language;
                existingCourse.Language = language;
            }
            _dbContext.Entry(existingCourse).CurrentValues.SetValues(course);
            _dbContext.SaveChanges();
            return existingCourse!;
        }

        public void Delete(string id)
        {
            var courseToDelete = _dbContext.Courses.Include(c => c.Language).FirstOrDefault(c => c.Id == id);
            if (courseToDelete != null)
            {
                _dbContext.Courses.Remove(courseToDelete);
                _dbContext.SaveChanges();
            }
        }
    }
}
