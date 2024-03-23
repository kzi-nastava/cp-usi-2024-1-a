using Consts;
using System;
using System.Collections.Generic;


public class Course
{
    private string name;
    private Language language;
    private LanguageLvl level;
    // representing the number of weeks
    private int duration;
    private List<WorkDay> schedule;
    private DateOnly start;
    private bool online;
    private int maxStudents;
    private int numStudents;
    private CourseState state;
    // attributes for reports
    private int numPenaltyPts;
    private int numStudentsPassed;
    private double readingAvgScore;
    private double writingAvgScore;
    private double listeningAvgScore;
    private double speakingAvgScore;

    public string Name { get; set; }
    public Language Language { get; set; }
    public LanguageLvl Level { get; set; }
    public int Duration { get; set; }
    public List<WorkDay> Schedule { get; set; }
    public DateOnly Start { get; set; }
    public bool Online { get; set; }
    public int MaxStudents { get; set; }
    public int NumStudents { get; set; }
    public CourseState State { get; set; }
    public int NumPenaltyPts { get; set; }
    public int NumStudentsPassed { get; set; }
    public double ReadingAvgScore { get; set; }
    public double WritingAvgScore { get; set; }
    public double ListeningAvgScore { get; set; }
    public double SpeakingAvgScore { get; set; }

    public Course(string name, Language language, LanguageLvl level, int duration, List<WorkDay> schedule, DateOnly start, bool online, int maxStudents, int numStudents, CourseState state)
    {
        Name = name;
        Language = language;
        Level = level;
        Duration = duration;
        Schedule = schedule;
        Start = start;
        Online = online;
        MaxStudents = maxStudents;
        NumStudents = numStudents;
        State = state;
        //set default values for attributes for reports
        NumPenaltyPts = 0;
        NumStudentsPassed = 0;
        ReadingAvgScore = 0;
        WritingAvgScore = 0;
        ListeningAvgScore = 0;
        SpeakingAvgScore = 0;
    }
    // Without maxStudents parameter, for online courses
    public Course(string name, Language language, LanguageLvl level, int duration, List<WorkDay> schedule, DateOnly start, bool online, CourseState state, int numStudents)
    {
        Name = name;
        Language = language;
        Level = level;
        Duration = duration;
        Schedule = schedule;
        Start = start;
        Online = online;
        MaxStudents = int.MaxValue;
        NumStudents = numStudents;
        State = state;
        //set default values for attributes for reports
        NumPenaltyPts = 0;
        NumStudentsPassed = 0;
        ReadingAvgScore = 0;
        WritingAvgScore = 0;
        ListeningAvgScore = 0;
        SpeakingAvgScore = 0;

    }


    public void AddAttendance()
    {
        NumStudents++;
    }

    public void CancelAttendance()
    {
        NumStudents--;
    }

    public bool IsFull()
    {
        return NumStudents == MaxStudents;
    }

    public bool CanBeUpdated()
    {
        if ((start.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber) > 7)
        {
            return true;
        }
        return false;
    }

    public bool UpdateCourse(string name, Language language, LanguageLvl level, int duration, List<WorkDay> schedule, DateOnly start, bool online, int maxStudents, int numStudents, CourseState state)
    {
        if(!CanBeUpdated())
        {
            return false;
        }
        if(name != null)
        {
            Name = name;
        }
        if(language != null)
        {
            Language = language;
        }
        if(level != 0)
        {
            Level = level;
        }
        if(duration != 0)
        {
            Duration = duration;
        }
        if(schedule != null)
        {
            Schedule = schedule;
        }
        if(string.IsNullOrEmpty(start.ToString()) == false)
        {
            Start = start;
        }
        if(online != false)
        {
            Online = online;
        }
        if(maxStudents != 0)
        {
            MaxStudents = maxStudents;
        }
        if(numStudents != 0)
        {
            NumStudents = numStudents;
        }
        if(state != 0)
        {
            State = state;
        }
        return true;
    }



}
