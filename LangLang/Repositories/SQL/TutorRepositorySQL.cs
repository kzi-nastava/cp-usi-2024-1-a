using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LangLang.Repositories.SQL
{
    public class TutorRepositorySQL : ITutorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TutorRepositorySQL(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Tutor> GetAll() => _dbContext.Tutors.ToList();

        public Tutor? Get(string id) => _dbContext.Tutors.FirstOrDefault(e => e.Id == id);

        public List<Tutor> Get(List<string> ids)
            => _dbContext.Tutors
                .Where(tutor => ids.Contains(tutor.Id))
                .ToList();

        public string GetId()
        {
            var lastTutor = _dbContext.Tutors.OrderByDescending(e => e.Id).FirstOrDefault();
            int nextId = (lastTutor != null) ? int.Parse(lastTutor.Id) + 1 : 1;
            return nextId.ToString();
        }

        public Tutor Add(Tutor tutor)
        {
            var existingTutor = _dbContext.Tutors.FirstOrDefault(e => e.Id == tutor.Id);

            if (existingTutor != null && tutor.Id != "-1")
            {
                _dbContext.Entry(existingTutor).CurrentValues.SetValues(tutor);
            }
            else
            {
                tutor.Id = GetId();
                _dbContext.Tutors.Add(tutor);
            }
            _dbContext.SaveChanges();
            return tutor;
        }

        public Tutor Update(string id, Tutor tutor)
        {
            var existingTutor = _dbContext.Tutors.FirstOrDefault(e => e.Id == tutor.Id);
            if (existingTutor != null)
            {
                _dbContext.Entry(existingTutor).CurrentValues.SetValues(tutor);
                _dbContext.SaveChanges();
            }
            return existingTutor!;
        }

        public void Delete(string id)
        {
            var tutorToDelete = _dbContext.Tutors.FirstOrDefault(e => e.Id == id);
            if (tutorToDelete != null)
            {
                _dbContext.Tutors.Remove(tutorToDelete);
                _dbContext.SaveChanges();
            }
        }

        public bool Exists(string id)
        {
            return Get(id) != null;
        }
    }
}
