using System;
using System.Collections.Generic;
using System.Linq;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Domain.Utility;

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
            => _dbContext.Exams.Where(exam => exam.Date == date).ToList();
        
        public List<Exam> GetForTimePeriod(DateTime from, DateTime to)
            => _dbContext.Exams.Where(exam => exam.Time >= from && exam.Time <= to).ToList();

        public List<Exam> GetByTutorId(string tutorId)
            => _dbContext.Exams.Where(exam => exam.TutorId == tutorId).ToList();

        public List<Exam> GetAllForPage(int pageNumber, int examsPerPage)
            => _dbContext.Exams
                .Skip((pageNumber - 1) * examsPerPage)
                .Take(examsPerPage)
                .ToList();

        public List<Exam> GetByTutorIdForPage(string tutorId, int pageNumber, int examsPerPage)
            => _dbContext.Exams
                .Where(exam => exam.TutorId == tutorId)
                .Skip((pageNumber - 1) * examsPerPage)
                .Take(examsPerPage)
                .ToList();

        public List<Exam> GetAll() => _dbContext.Exams.ToList();

        public Exam? Get(string id) => _dbContext.Exams?.Find(id);

        public List<Exam> Get(List<string> ids)
            => _dbContext.Exams.Where(exam => ids.Contains(exam.Id)).ToList();

        public string GetId()
        {
            var lastExam = _dbContext.Exams.OrderByDescending(c => c.Id).FirstOrDefault();
            int nextId = (lastExam != null) ? int.Parse(lastExam.Id) + 1 : 1;
            return nextId.ToString();
        }

        public Exam Add(Exam exam)
        {
            var existingExam = _dbContext.Exams.FirstOrDefault(c => c.Id == exam.Id);

            if (existingExam != null && exam.Id != "-1")
            {
                _dbContext.Entry(existingExam).CurrentValues.SetValues(exam);
                _dbContext.SaveChanges();
            }
            else
            {
                exam.Id = GetId();
                _dbContext.Exams.Add(exam);
                _dbContext.SaveChanges();
            }
            return exam;
        }

        public Exam Update(string id, Exam exam)
        {
            var existingExam = _dbContext.Exams.Find(id);
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
