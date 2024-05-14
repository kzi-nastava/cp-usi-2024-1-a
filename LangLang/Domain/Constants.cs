using System;

namespace LangLang.Domain;

public static class Constants
{
    public const int ClassroomsNumber = 2;
        
    public static readonly TimeSpan ExamDuration = TimeSpan.FromHours(4);
    public static readonly TimeSpan LessonDuration = TimeSpan.FromMinutes(90);
        
    public static readonly TimeSpan LockedExamTime = new(14, 0, 0, 0);
    public static readonly TimeSpan ConfirmedExamTime = new(7, 0, 0, 0);
    public static readonly TimeSpan ConfirmableCourseTime = new(7, 0, 0, 0);
    public static readonly TimeSpan CancellableCourseTime = new(7, 0, 0, 0);
    
    public const uint PenaltyPointLimit = 3;

    public const uint MaxReadingScore = 60;
    public const uint MaxWritingScore = 60;
    public const uint MaxListeningScore = 40;
    public const uint MaxSpeakingScore = 50;
    public const uint MinPassingScore = 160;        
}