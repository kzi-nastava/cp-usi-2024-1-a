using System;
using System.Collections.Generic;
using System.IO;
using Consts;
using LangLang.Model;
using LangLang.Util;

namespace LangLang.DAO.JsonDao
{
    public class CourseApplicationDAO : ICourseApplicationDAO
    {
        private Dictionary<string, CourseApplication>? _courseApplications;

        private Dictionary<string, CourseApplication> CourseApplications
        {
            get
            {
                if (_courseApplications == null)
                {
                    Load();
                }
                return _courseApplications!;
            }
            set { _courseApplications = value; }
        }

        private readonly ILastIdDAO _lastIdDAO;

        public CourseApplicationDAO(ILastIdDAO lastIdDao)
        {
            _lastIdDAO = lastIdDao;
        }

        public Dictionary<string, CourseApplication> GetAll()
        {
            return CourseApplications;
        }









        private void Load()
        {
            try
            {
                _courseApplications = JsonUtil.ReadFromFile<CourseApplication>(Constants.CourseApplicationsFilePath);
            }
            catch (DirectoryNotFoundException)
            {
                CourseApplications = new Dictionary<string, CourseApplication>();
                Save();
            }
            catch (FileNotFoundException)
            {
                CourseApplications = new Dictionary<string, CourseApplication>();
                Save();
            }
        }
        private void Save()
        {
            JsonUtil.WriteToFile<CourseApplication>(CourseApplications, Constants.CourseApplicationsFilePath);
        }
    }
}
