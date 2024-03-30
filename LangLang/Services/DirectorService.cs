using Consts;
using System;
using System.Collections.Generic;
using System.Windows;
using LangLang.Model;
using LangLang.DAO;

namespace LangLang.Services
{
    public class DirectorService
    {
        DirectorDAO directorDAO = DirectorDAO.GetInstance();
        public Director LoggedUser { get; set; }


        //Singleton
        private static DirectorService instance;
        private DirectorService()
        {
        }

        public static DirectorService GetInstance()
        {
            if (instance == null)
                instance = new DirectorService();
            return instance;
        }
    }
}