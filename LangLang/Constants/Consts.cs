using System;

namespace Consts
{
    public static class Constants
    {
        public const int ClassroomsNumber = 2;
        
        public static readonly TimeSpan ExamDuration = TimeSpan.FromHours(4);
        public static readonly TimeSpan LessonDuration = TimeSpan.FromMinutes(90);
        
        public static readonly TimeSpan LockedExamTime = new(14, 0, 0, 0);
        public static readonly TimeSpan ConfirmedExamTime = new(7, 0, 0, 0);
        public static readonly TimeSpan ConfirmableCourseTime = new(7, 0, 0, 0);
        public static readonly TimeSpan CancellableCourseTime = new(7, 0, 0, 0);
        public static readonly string DateFormat = "yyyy-MM-dd";

        public const uint PenaltyPointLimit = 3;

        public const uint MaxReadingScore = 60;
        public const uint MaxWritingScore = 60;
        public const uint MaxListeningScore = 40;
        public const uint MaxSpeakingScore = 50;
        public const uint MinPassingScore = 160;

        public const string StudentFilePath = "../../../Data/Students.json";
        public const string CourseFilePath = "../../../Data/Courses.json";
        public const string ExamFilePath = "../../../Data/Exams.json";
        public const string LanguageFilePath = "../../../Data/Languages.json";
        public const string TutorFilePath = "../../../Data/Tutors.json";
        public const string DirectorFilePath = "../../../Data/Directors.json";
        public const string LastIdFilePath = "../../../Data/LastId.json";
        public const string ProfileFilePath = "../../../Data/Profiles.json";
        public const string PersonProfileMappingFilePath = "../../../Data/PersonProfile.json";
        public const string CourseApplicationsFilePath = "../../../Data/CourseApplications.json";
        public const string CourseAttendancesFilePath = "../../../Data/CourseAttendances.json";
        public const string NotificationFilePath = "../../../Data/Notifications.json";
        public const string DropRequestFilePath = "../../../Data/DropRequests.json";
        public const string ExamApplicationFilePath = "../../../Data/ExamApplications.json";
        public const string ExamAttendancesFilePath = "../../../Data/ExamAttendances.json";
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
    public static class Extensions
    {
        public static string ToStr(this LanguageLvl level)
        {
            return level switch
            {
                LanguageLvl.A1 => "A1",
                LanguageLvl.A2 => "A2",
                LanguageLvl.B1 => "B1",
                LanguageLvl.B2 => "B2",
                LanguageLvl.C1 => "C1",
                LanguageLvl.C2 => "C2",
                _ => "",
            };
        }
    }
    public enum WorkDay
    {
        Monday,Tuesday,Wednesday,Thursday,Friday
    }
    public enum EducationLvl
    {
        ElementarySchool, HighSchool, CollegeDegree, MastersDegree, PhD
    }
}

