﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LangLang.Core;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Application.UseCases.Course
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILanguageRepository _languageRepository;

        public CourseService(ICourseRepository courseRepository, ILanguageRepository languageRepository)
        {
            _courseRepository = courseRepository;
            _languageRepository = languageRepository;
        }


        public List<Domain.Model.Course> GetAll()
        {
            return _courseRepository.GetAll();
        }

        public List<Domain.Model.Course> GetCoursesByTutor(Tutor tutor)
        {
            return _courseRepository.GetByTutorId(tutor.Id);
        }
        
        public List<Domain.Model.Course> GetAvailableCourses(Student student)
        {
            List<Domain.Model.Course> courses = new();
            foreach (Domain.Model.Course course in GetAll())
            {
                if (course.IsApplicable())
                {
                    courses.Add(course);
                }
            }

            return courses;
        }

        public void AddCourse(Domain.Model.Course course, bool isCreatedByTutor = true)
        {
            course.IsCreatedByTutor = isCreatedByTutor;
            _courseRepository.Add(course);
        }

        public Domain.Model.Course? GetCourseById(string id)
        {
            return _courseRepository.Get(id);
        }

        public void DeleteCourse(string id)
        {
            _courseRepository.Delete(id);
        }

        public void UpdateCourse(Domain.Model.Course course)
        {
            _courseRepository.Update(course.Id, course);
        }

        public void FinishCourse(string id)
        {
            Domain.Model.Course? course = GetCourseById(id);
            if (course == null)
            {
                throw new ArgumentException("Course not found");
            }

            course.Finish();
            UpdateCourse(course);
        }

        public void AddAttendance(string courseId)
        {
            GetCourseById(courseId)!.AddAttendance();
            UpdateCourse(GetCourseById(courseId)!);
        }

        public void CancelAttendance(string courseId)
        {
            GetCourseById(courseId)!.CancelAttendance();
            UpdateCourse(GetCourseById(courseId)!);
        }

        public Domain.Model.Course? ValidateInputs(Tutor? tutor, string name, string? languageName, LanguageLevel? level,
            int? duration, Dictionary<WorkDay, Tuple<TimeOnly, int>> schedule,
            ObservableCollection<WorkDay> scheduleDays, DateTime? start, bool online, int numStudents,
            Domain.Model.Course.CourseState? state, int maxStudents)
        {
            if (!Domain.Model.Course.IsValid(name, languageName, level, duration, state, start, maxStudents,
                    scheduleDays))
            {
                return null;
            }

            Language? language = _languageRepository.Get(languageName!);
            if (language == null)
            {
                return null;
            }

            if (tutor == null) return null;

            if (online)
            {
                maxStudents = int.MaxValue;
            }

            return new Domain.Model.Course(
                tutor.Id,
                name,
                language,
                (LanguageLevel)level!,
                (int)duration!,
                schedule,
                (DateTime)start!,
                online,
                numStudents,
                (Domain.Model.Course.CourseState)state!,
                maxStudents
            );
        }

        public List<Domain.Model.Course> GetCoursesForLastYear() =>
            _courseRepository.GetForTimePeriod(DateTime.Now.AddYears(-1), DateTime.Now);

        public void RemoveTutorFromAllCourses(Tutor tutor)
        {
            foreach (var course in GetCoursesByTutor(tutor))
            {
                course.TutorId = null;
                _courseRepository.Update(course.Id, course);
            }
        }

        public Domain.Model.Course? SetTutor(Domain.Model.Course course, Tutor tutor)
        {
            course.TutorId = tutor.Id;
            return _courseRepository.Update(course.Id, course);
        }

        public void UpdateStates()
        {
            foreach (Domain.Model.Course course in GetAll())
            {
                course.UpdateState();
                UpdateCourse(course);
            }
        }

        public List<Domain.Model.Course> FilterCoursesForPage(int pageNumber, int coursesPerPage, string? language = null, LanguageLevel? languageLevel = null, DateTime? start = null, bool? online = null, int? duration = null)
        {
            return FilterCourses(language, languageLevel,start, online, duration).GetPage(pageNumber, coursesPerPage);
        }

        public List<Domain.Model.Course> FilterCourses(string? language = null, LanguageLevel? languageLvl = null, DateTime? start = null, bool? online = null, int? duration = null)
        {
            return GetAll().Where(course => course.MatchesFilter(language, languageLvl, start, online, duration)).ToList();
        }

        public List<Domain.Model.Course> GetAllCoursesForPage(int pageNumber, int coursesPerPage)
        {
            return _courseRepository.GetAllForPage(pageNumber, coursesPerPage);
        }

        public List<Domain.Model.Course> GetCoursesByTutorForPage(string tutorId, int pageNumber, int coursesPerPage)
        {
            return _courseRepository.GetByTutorIdForPage(tutorId, pageNumber, coursesPerPage);
        }
    }
}