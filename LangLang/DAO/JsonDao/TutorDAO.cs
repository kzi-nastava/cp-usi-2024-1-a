using System.Collections.Generic;
using Consts;
using LangLang.Model;
using LangLang.Util;

namespace LangLang.DAO.JsonDao
{
    public class TutorDAO : ITutorDAO
    {
        private ILastIdDAO _lastIdDao;
        
        private Dictionary<string, Tutor>? _tutors;

        public TutorDAO(ILastIdDAO lastIdDao)
        {
            _lastIdDao = lastIdDao;
        }

        private Dictionary<string, Tutor> Tutors
        {
            get
            {
                if (_tutors == null)
                    Load();
                return _tutors!;
            }
            set => _tutors = value;
        }

        public Dictionary<string, Tutor> GetAllTutors() => Tutors;

        public Tutor? GetTutor(string id)
        {
            return Tutors.GetValueOrDefault(id);
        }

        public Tutor AddTutor(Tutor tutor)
        {
            _lastIdDao.IncrementTutorId();
            tutor.Id = _lastIdDao.GetTutorId();
            Tutors.Add(tutor.Id, tutor);
            Save();
            return tutor;
        }

        public void UpdateTutor(Tutor tutor)
        {
            if (Tutors.ContainsKey(tutor.Id))
            {
                Tutors[tutor.Id] = tutor;
                Save();
            }
        }

        public bool Exists(string id) => Tutors.ContainsKey(id);

        public void DeleteTutor(string id)
        {
            Tutors.Remove(id);
            Save();
        }

        private void Load()
        {
            try
            {
                _tutors = JsonUtil.ReadFromFile<Tutor>(Constants.TutorFilePath);
            }
            catch
            {
                Tutors = new();
                Save();
            }
        }

        private void Save() => JsonUtil.WriteToFile(Tutors, Constants.TutorFilePath);
    }
}