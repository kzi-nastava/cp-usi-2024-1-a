using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LangLang.Repositories.SQL
{
    public class ExamRepositorySQL : IExamRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ExamRepositorySQL(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Exam> GetByDate(DateOnly date)
        {
            return _dbContext.Exams
                .Where(exam => exam.Time.Date == date.ToDateTime(TimeOnly.MinValue).Date)
                               .ToList();
        }

        public List<Exam> GetForTimePeriod(DateTime from, DateTime to)
            => _dbContext.Exams.Where(exam => exam.Time >= from && exam.Time <= to).ToList();

        public List<Exam> GetByTutorId(string tutorId)
            => _dbContext.Exams.Where(exam => exam.TutorId == tutorId).ToList();

        public List<Exam> GetAllForPage(int pageNumber, int examsPerPage)
            => _dbContext.Exams
                .Skip((pageNumber - 1) * examsPerPage)
                .Take(examsPerPage)
                .ToList();
        }

        public List<Exam> GetByTutorIdForPage(string tutorId, int pageNumber, int examsPerPage)
            => _dbContext.Exams
                .Where(exam => exam.TutorId == tutorId)
                .Skip((pageNumber - 1) * examsPerPage)
                .Take(examsPerPage)
                .ToList();
        }

        public List<Exam> GetAll() => _dbContext.Exams.ToList();

        public Exam? Get(string id) => _dbContext.Exams?.Find(id);

        public List<Exam> Get(List<string> ids)
            => _dbContext.Exams.Where(exam => ids.Contains(exam.Id)).ToList();

        public string GetId()
        {
            var lastExam = _dbContext.Exams.OrderByDescending(e => e.Id).FirstOrDefault();
            int nextId = (lastExam != null) ? int.Parse(lastExam.Id) + 1 : 1;
            return nextId.ToString();
        }

        public Exam Add(Exam exam)
        {
            var existingExam = _dbContext.Exams.AsNoTracking().FirstOrDefault(e => e.Id == exam.Id);
            var language = _dbContext.Languages.Find(exam.Language.Name);

            if (language != null)
            {
                exam.Language = language;
            }

            if (existingExam != null && exam.Id != "-1")
            {
                _dbContext.Entry(existingExam).CurrentValues.SetValues(exam);
            }
            else
            {
                exam.Id = GetId();
                _dbContext.Exams.Add(exam);
            }
            _dbContext.SaveChanges();
            return exam;
        }

        public Exam Update(string id, Exam exam)
        {
            var existingExam = _dbContext.Exams.Find(id);
            var language = _dbContext.Languages.Find(exam.Language.Name);

            if (language != null)
            {
                exam.Language = language;
            }

            if (existingExam != null)
            {
                _dbContext.Entry(existingExam).CurrentValues.SetValues(exam);
                _dbContext.SaveChanges();
            }
            return existingExam!;
        }

        public void Delete(string id)
        {
            var examToDelete = _dbContext.Exams.Find(id);
            if (examToDelete != null)
            {
                _dbContext.Exams.Remove(examToDelete);
                _dbContext.SaveChanges();
            }
        }
    }
}
