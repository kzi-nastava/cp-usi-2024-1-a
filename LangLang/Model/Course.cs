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

    public Course()
    {
        Name = "";
        Language = new Language("English", "en");
        Level = LanguageLvl.A1;
        Duration = 0;
        Schedule = new List<WorkDay> { WorkDay.MON };
        Start = DateOnly.MaxValue;
        Online = false;
        MaxStudents = 0;
        NumStudents = 0;
        State = CourseState.ACTIVE;
        //set default values for attributes for reports
        NumPenaltyPts = 0;
        NumStudentsPassed = 0;
        ReadingAvgScore = 0;
        WritingAvgScore = 0;
        ListeningAvgScore = 0;
        SpeakingAvgScore = 0;
    }

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

   


}
