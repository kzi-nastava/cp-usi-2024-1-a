using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Consts;
using LangLang.Model;

namespace LangLang.Services.CourseServices;

public interface ICourseService
{
    public Dictionary<string, Course> GetAll();
    public Dictionary<string, Course> GetCoursesByTutor(Tutor loggedInUser);
    public List<Course> GetAvailableCourses(Student student);
    public void AddCourse(Course course, Tutor loggedInUser);
    public Course? GetCourseById(string id);
    public void DeleteCourse(string id, Tutor loggedInUser);
    public void UpdateCourse(Course course);
    public void FinishCourse(string id);
    public void CalculateAverageScores(string id);

    public Course? ValidateInputs(string name, string? languageName, LanguageLvl? level, int? duration,
        Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule, ObservableCollection<WorkDay> scheduleDays, DateTime? start,
        bool online, int numStudents, CourseState? state, int maxStudents);
}