using Consts;
using System;
using System.Collections.Generic;
using System.Windows;
using LangLang.Model;
using LangLang.DAO;

namespace LangLang.Services
{
    public class TutorService
    {
        TutorDAO tutorDAO = TutorDAO.GetInstance();
        public Tutor LoggedUser { get; set; }


        //Singleton
        private static TutorService instance;
        private TutorService()
        {
        }

        public static TutorService GetInstance()
        {
            if (instance == null)
                instance = new TutorService();
            return instance;
        }
    }
}