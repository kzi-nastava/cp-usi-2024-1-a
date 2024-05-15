using System;
using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface ICourseRepository : IRepository<Course>
{
    public List<Course> GetCoursesByDate(DateOnly date);
}