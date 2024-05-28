using System;
using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface ICourseRepository : IRepository<Course>
{
    public List<Course> GetCoursesByDate(DateOnly date);
    public List<Course> GetForTimePeriod(DateTime from, DateTime to);
    public List<Course> GetByTutorId(string tutorId);
    public List<Course> GetAllForPage(int pageNumber, int coursesPerPage);
    public List<Course> GetByTutorIdForPage(string tutorId, int pageNumber, int coursesPerPage);
}