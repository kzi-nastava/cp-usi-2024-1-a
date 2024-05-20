using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.Course;

public interface ICourseService
{
    public List<Domain.Model.Course> GetAll();
    public List<Domain.Model.Course> GetCoursesByTutor(Tutor loggedInUser);
    public List<Domain.Model.Course> GetAvailableCourses(Student student);
    public void AddCourse(Domain.Model.Course course);
    public Domain.Model.Course? GetCourseById(string id);
    public void DeleteCourse(string id);
    public void UpdateCourse(Domain.Model.Course course);
    public void FinishCourse(string id);
    public void AddAttendance(string courseId);
    public void CancelAttendance(string courseId);
    public void UpdateStates();
    public Domain.Model.Course? ValidateInputs(Tutor? tutor, string name, string? languageName, LanguageLevel? level, int? duration,
        Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule, ObservableCollection<WorkDay> scheduleDays, DateTime? start,
        bool online, int numStudents, Domain.Model.Course.CourseState? state, int maxStudents);
    List<Domain.Model.Course> GetCoursesForLastYear();
}