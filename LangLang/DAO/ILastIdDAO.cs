using System;

namespace LangLang.DAO;

public interface ILastIdDAO
{
    public String GetCourseId();
    public void IncrementCourseId();
    public String GetExamId();
    public void IncrementExamId();
}