﻿using System;

namespace LangLang.DAO;

public interface ILastIdDAO
{
    public string GetCourseId();
    public void IncrementCourseId();
    public string GetExamId();
    public void IncrementExamId();
    public string GetStudentId();
    public void IncrementStudentId();
    public string GetTutorId();
    public void IncrementTutorId();
    public string GetCourseAttendanceId();
    public void IncrementCourseAttendanceId();
    public string GetCourseApplicationId();
    public void IncrementCourseApplicationId();
}