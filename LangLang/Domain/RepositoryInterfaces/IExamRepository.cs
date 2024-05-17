using System;
using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface IExamRepository : IRepository<Exam>
{
    public List<Exam> GetByDate(DateOnly date);
    List<Exam> GetForTimePeriod(DateTime from, DateTime to);
}