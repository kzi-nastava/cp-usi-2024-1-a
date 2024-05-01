using System.Collections.Generic;
using Consts;
using LangLang.Model;
using LangLang.Util;

namespace LangLang.DAO.JsonDao
{
    public class TutorDAO : ITutorDAO
    {
        private Dictionary<string, Tutor>? _tutors;

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

        public Tutor? GetTutor(string email)
        {
            return Tutors.GetValueOrDefault(email);
        }

        public void AddTutor(Tutor tutor)
        {
            Tutors.Add(tutor.Id, tutor);
            Save();
        }

        public void UpdateTutor(Tutor tutor)
        {
            if (Tutors.ContainsKey(tutor.Id))
            {
                Tutors[tutor.Id] = tutor;
                Save();
            }
        }

        public bool Exists(string email) => Tutors.ContainsKey(email);

        public void DeleteTutor(string email)
        {
            Tutors.Remove(email);
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