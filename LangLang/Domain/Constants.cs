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

    public const string ResultEmailSubject = "{0} exam results";
    public const string ResultEmailBodyFail = @"Dear {0},

You have taken part in {1} exam on {2}. Unfortunatly, you have failed. However, don't worry, because you can always try again.

Warm regards,

LangLang school";

    public const string ResultEmailBodyPass = @"Dear {0},

You have taken part in {1} exam on {2}. Congratulations, you have passed.
Your grade on this exam is:
    - Reading: {3}
    - Writing: {4}
    - Listening: {5}
    - Speaking: {6}

Warm regards,

LangLang school";
}