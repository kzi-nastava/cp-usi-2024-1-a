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
            LastId.ExamId++;
            Save();
        }

        private void Load()
        {
            Dictionary<string, LastId> ret = JsonUtil.ReadFromFile<LastId>(Constants.LastIdFilePath);
            if (ret.Count <= 0)
            {
                LastId = new LastId();
                Save();
            }
            else
            {
                LastId = ret[jsonKey];
            }
        }

        private void Save()
        {
            Dictionary<string, LastId> dict = new Dictionary<string, LastId> { { jsonKey, LastId } };
            JsonUtil.WriteToFile(dict, Constants.LastIdFilePath);
        }
    }
}