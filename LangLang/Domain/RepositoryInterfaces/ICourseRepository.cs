using System;
using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface ICourseRepository : IRepository<Course>
{
    public List<Course> GetCoursesByDate(DateOnly date);
    public List<Course> GetForTimePeriod(DateTime from, DateTime to);
    public List<Course> GetByTutorId(string tutorId);
}