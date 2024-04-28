using LangLang.Model;
using System.Collections.Generic;
using System;

namespace LangLang.DAO;
public interface ICourseApplicationDAO
{
    public Dictionary<string, CourseApplication> GetAll();
}
