using System;
using System.Collections.Generic;
using System.IO;
using Consts;
using LangLang.Model;
using LangLang.Util;

namespace LangLang.DAO
{
    public class LastIdDAO
    {
        private const String jsonKey = "ids";

        private static LastIdDAO? instance;

        private LastId? lastId;

        private LastId LastId
        {
            get
            {
                if (lastId == null)
                {
                    Load();
                }

                return lastId!;
            }
            set => lastId = value;
        }

        private LastIdDAO(){}

        public static LastIdDAO GetInstance()
        {
            return instance ??= new LastIdDAO();
        }

        public String GetCourseId()
        {
            return LastId.CourseId.ToString();
        }

        public void IncrementCourseId()
        {
            LastId.CourseId++;
            Save();
        }

        public String GetExamId()
        {
            return LastId.ExamId.ToString();
        }

        public void IncrementExamId()
        {
            LastId.CourseId++;
            Save();
        }

        private void Load()
        {
            try
            {
                lastId = JsonUtil.ReadFromFile<LastId>(Constants.LastIdFilePath)[jsonKey];
            }
            catch (DirectoryNotFoundException e)
            {
                LastId = new LastId();
                Save();
            }
            catch (FileNotFoundException e)
            {
                LastId = new LastId();
                Save();
            }
        }

        private void Save()
        {
            Dictionary<string, LastId> dict = new Dictionary<string, LastId> { { jsonKey, LastId } };
            JsonUtil.WriteToFile(dict, Constants.LastIdFilePath);
        }
    }
}