using System;

namespace Consts
{
    public static class Constants
    {
        public const int LessonDuration = 90;   //minutes
        public const int ExamDuration = 4;  //hours
        public const int ClassroomNumber = 2;
    
    }

    public enum Gender
    {
        Female, Male, Other
    }

    public enum Role
    {
        Student, Tutor, Director
    }

    public enum LanguageLvl
    {
        A1, A2, B1, B2, C1, C2
    }

    public enum CourseState
    {
        ACTIVE, CANCELED, FINISHED
    }
    public enum WorkDay
    {
        MON,TUE,WED,THU,FRI
    }


    
}

