﻿namespace LangLang.Domain.Model
{
    public class CourseApplication : IEntity
    {
        public string Id { get; set; }
        public enum State
        {
            Pending, Rejected, Accepted, Paused
        }
        public string CourseId { get; set; }
        public string StudentId { get; set; }
        public State CourseApplicationState { get; set; }
        public CourseApplication()
        {
            Id = "";
            CourseId = "";
            StudentId = "";
            CourseApplicationState = State.Pending;

        }

        public CourseApplication(string studentId, string courseId, State courseApplicationState)
        {
            Id = "";
            CourseId = courseId;
            StudentId = studentId;
            CourseApplicationState = courseApplicationState;
        }

        public CourseApplication(string id,string courseId, string studentId, State courseApplicationState)
        {
            Id = id;
            CourseId = courseId;
            StudentId = studentId;
            CourseApplicationState = courseApplicationState;
        }
        public void ChangeApplicationState(State courseApplicationState)
        {
            CourseApplicationState = courseApplicationState;
        }


    }
}
