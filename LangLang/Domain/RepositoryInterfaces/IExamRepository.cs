using System;
using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface IExamRepository : IRepository<Exam>
{
    public List<Exam> GetByDate(DateOnly date);
    public List<Exam> GetForTimePeriod(DateTime from, DateTime to);
    public List<Exam> GetByTutorId(string tutorId);
}