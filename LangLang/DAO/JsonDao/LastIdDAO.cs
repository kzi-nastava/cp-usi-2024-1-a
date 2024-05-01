﻿using System;
using System.Collections.Generic;
using Consts;
using LangLang.Model;
using LangLang.Util;

namespace LangLang.DAO.JsonDao
{
    public class LastIdDAO : ILastIdDAO
    {
        private const String JsonKey = "ids";
        
        private LastId? _lastId;

        private LastId LastId
        {
            get
            {
                if (_lastId == null)
                {
                    Load();
                }

                return _lastId!;
            }
            set => _lastId = value;
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

        public string GetStudentId()
        {
            return LastId.StudentId.ToString();
        }

        public void IncrementStudentId()
        {
            LastId.StudentId++;
            Save();
        }

        public string GetTutorId()
        {
            return LastId.TutorId.ToString();
        }

        public void IncrementTutorId()
        {
            LastId.TutorId++;
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
                LastId = ret[JsonKey];
            }
        }

        private void Save()
        {
            Dictionary<string, LastId> dict = new Dictionary<string, LastId> { { JsonKey, LastId } };
            JsonUtil.WriteToFile(dict, Constants.LastIdFilePath);
        }
    }
}