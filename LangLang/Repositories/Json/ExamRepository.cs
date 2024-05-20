using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Repositories.Json;

public class ExamRepository : AutoIdRepository<Exam>, IExamRepository
{
    public ExamRepository(string filepath, string lastIdFilePath) : base(filepath, lastIdFilePath)
    {
    }
    
    public List<Exam> GetByDate(DateOnly date)
    {
        return GetAll().Where(exam => exam.Date == date).ToList();
    }

    public List<Exam> GetForTimePeriod(DateTime from, DateTime to)
    {
        return GetAll().Where(exam => exam.Time >= from && exam.Time <= to).ToList();
    }

    public List<Exam> GetByTutorId(string tutorId)
    {
        return GetAll().Where(exam => exam.TutorId == tutorId).ToList();
    }
}