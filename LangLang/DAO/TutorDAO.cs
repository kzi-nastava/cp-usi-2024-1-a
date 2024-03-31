using Consts;
using System.Collections.Generic;
using LangLang.Model;
using LangLang.Util;
using System.Windows.Navigation;
using Microsoft.Win32;
using System.IO;

namespace LangLang.DAO
{
    public class TutorDAO
    {
        private static TutorDAO? instance;
        private Dictionary<string, Tutor>? tutors;

        private Dictionary<string, Tutor> Tutors
        {
            get
            {
                if (tutors == null)
                    Load();
                return tutors!;
            }
            set => tutors = value;
        }

        private TutorDAO()
        {
        }

        public static TutorDAO GetInstance()
        {
            return instance ??= new TutorDAO();
        }

        public Dictionary<string, Tutor> GetAllTutors() => Tutors;

        public Tutor? GetTutor(string email)
        {
            return Tutors.GetValueOrDefault(email);
        }

        public void AddTutor(Tutor tutor)
        {
            Tutors.Add(tutor.Email, tutor);
            Save();
        }

        public void UpdateTutor(string email, Tutor tutor)
        {
            if (Tutors.ContainsKey(email))
            {
                Tutors[email] = tutor;
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
                tutors = JsonUtil.ReadFromFile<Tutor>(Constants.TutorFilePath);
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