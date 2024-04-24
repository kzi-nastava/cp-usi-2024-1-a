using System;
using System.Collections.Generic;
using System.Linq;
using Consts;
using LangLang.DAO;
using LangLang.Model;
using LangLang.Util;
using TimeOnly = System.TimeOnly;

namespace LangLang.Services.UtilityServices;

public class TimetableService : ITimetableService
{
    private readonly ICourseDAO _courseDao;
    private readonly IExamDAO _examDao;

    public TimetableService(ICourseDAO courseDao, IExamDAO examDao)
    {
        _courseDao = courseDao;
        _examDao = examDao;
    }

    private List<TimeOnly> GetAllExamTimes()
    {
        int totalNumber = Convert.ToInt32(Math.Round(TimeSpan.FromDays(1).TotalMinutes / Constants.ExamDuration.TotalMinutes));
        
        List<TimeOnly> times = new();
        TimeOnly time = TimeOnly.MinValue;
        for (int i=0; i<totalNumber; i++)
        {
            times.Add(time);
            time = time.Add(Constants.ExamDuration);
        }

        return times;
    }

    private List<TimeOnly> GetAllLessonTimes()
    {
        int totalNumber = Convert.ToInt32(Math.Round(TimeSpan.FromDays(1).TotalMinutes / Constants.LessonDuration.TotalMinutes));
        
        List<TimeOnly> times = new();
        TimeOnly time = TimeOnly.MinValue;
        for (int i=0; i<totalNumber; i++)
        {
            times.Add(time);
            time = time.Add(Constants.LessonDuration);
        }

        return times;
    }
    
    public List<TimeOnly> GetAvailableExamTimes(DateOnly date, Tutor tutor)
    {
        List<TimeOnly> candidateTimes = GetAllExamTimes();
        Dictionary<int, List<Tuple<TimeOnly, TimeSpan>>> takenTimes = GetTakenTimes(date, tutor);

        HashSet<TimeOnly> availableTimes = new HashSet<TimeOnly>();
        for (int i = 1; i <= Constants.ClassroomsNumber; i++)
        {
            var availableForClassroom= CalculateAvailableTimes(candidateTimes, Constants.ExamDuration, takenTimes[i]);
            availableTimes.UnionWith(availableForClassroom);
        }

        List<TimeOnly> result = availableTimes.ToList();
        result.Sort();
        return result;
    }

    public Dictionary<WorkDay, List<TimeOnly>> GetAvailableLessonTimes(DateOnly start, int numOfWeeks, Tutor tutor)
    {
        Dictionary<WorkDay, List<TimeOnly>> availableTimes = new();
        for (int i = 0; i < numOfWeeks * 7; i++)
        {
            DateOnly date = start.AddDays(i);
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                continue;
            }
            List<TimeOnly> candidateTimes = GetAllLessonTimes();
            Dictionary<int, List<Tuple<TimeOnly, TimeSpan>>> takenTimes = GetTakenTimes(date, tutor);
            WorkDay day = DayConverter.ToWorkDay(date.DayOfWeek);
            for (int j = 1; j <= Constants.ClassroomsNumber; j++)
            {
                var availableForClassroom = CalculateAvailableTimes(candidateTimes, Constants.LessonDuration, takenTimes[j]);
                if (!availableTimes.TryAdd(day, availableForClassroom))
                {
                    availableTimes[day] = Intersection(availableTimes[day], availableForClassroom);
                }
            }
        }

        return availableTimes;
    }
    
    public List<int> GetAvailableClassrooms(DateOnly date, TimeOnly time, TimeSpan duration, Tutor tutor)
    {
        Dictionary<int, List<Tuple<TimeOnly, TimeSpan>>> takenTimes = GetTakenTimes(date, tutor);
        List<int> availableClassrooms = new();
        for (int i = 1; i <= Constants.ClassroomsNumber; i++)
        {
            bool taken = false;
            foreach (var takenTime in takenTimes[i])
            {
                if (time.Add(duration) > takenTime.Item1 && time < takenTime.Item1.Add(takenTime.Item2))
                {
                    taken = true;
                }
            }

            if (!taken)
            {
                availableClassrooms.Add(i);
            }
        }

        return availableClassrooms;
    }

    private  Dictionary<int, List<Tuple<TimeOnly, TimeSpan>>> GetTakenTimes(DateOnly date, Tutor tutor)
    {
        Dictionary<int, List<Tuple<TimeOnly, TimeSpan>>> takenTimes = new();
        for (int i = 1; i <= Constants.ClassroomsNumber; i++)
        {
            takenTimes.Add(i, new List<Tuple<TimeOnly, TimeSpan>>());
        }
        
        List<Exam> exams = _examDao.GetExamsByDate(date);
        foreach (Exam exam in exams)
        {
            // if the current tutor is busy in one classroom, mark all other classrooms as taken
            if (tutor.Exams.Contains(exam.Id))
            {
                for (int j = 1; j <= Constants.ClassroomsNumber; j++)
                {
                    takenTimes[j].Add(new Tuple<TimeOnly, TimeSpan>(exam.TimeOfDay, Constants.ExamDuration));
                }
            }
            else
            {
                takenTimes[exam.ClassroomNumber].Add(new Tuple<TimeOnly, TimeSpan>(exam.TimeOfDay, Constants.ExamDuration));
            }
        }

        List<Course> courses = _courseDao.GetCoursesByDate(date);
        foreach (Course course in courses)
        {
            if(course.Online) continue;
            Tuple<TimeOnly, int> timeAndClassroom = course.Schedule[DayConverter.ToWorkDay(date.DayOfWeek)];
            if (tutor.Courses.Contains(course.Id))
            {
                for (int j = 1; j <= Constants.ClassroomsNumber; j++)
                {
                    takenTimes[j].Add(new Tuple<TimeOnly, TimeSpan>(timeAndClassroom.Item1, Constants.LessonDuration));
                }
            }
            else
            {
                takenTimes[timeAndClassroom.Item2]
                    .Add(new Tuple<TimeOnly, TimeSpan>(timeAndClassroom.Item1, Constants.LessonDuration));
            }
        }

        return takenTimes;
    }

    private List<TimeOnly> CalculateAvailableTimes(List<TimeOnly> candidateTimes, TimeSpan duration, List<Tuple<TimeOnly, TimeSpan>> takenTimes)
    {
        List<TimeOnly> availableTimes = new();
        int i = 0, j = 0;
        while (i < candidateTimes.Count && j < takenTimes.Count)
        {
            if (takenTimes[j].Item1.Add(takenTimes[j].Item2) <= candidateTimes[i])
            {
                j++;
            } 
            else if(takenTimes[j].Item1 < candidateTimes[i].Add(duration))
            {
                i++;
            } 
            else
            {
                availableTimes.Add(candidateTimes[i]);
                i++;
            }
        }

        while (i < candidateTimes.Count)
        {
            availableTimes.Add(candidateTimes[i]);
            i++;
        }
        
        return availableTimes;
    }
    
    private List<TimeOnly> Intersection(List<TimeOnly> list1, List<TimeOnly> list2)
    {
        list1.Sort();
        list2.Sort();
        List<TimeOnly> result = new();
        int i = 0, j = 0;
        while (i < list1.Count && j < list2.Count)
        {
            if (list1[i] == list2[j])
            {
                result.Add(list1[i]);
                i++;
                j++;
            }
            else if (list1[i] < list2[j])
            {
                i++;
            }
            else
            {
                j++;
            }
        }

        return result;
    }
}