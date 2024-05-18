using System.Collections.Generic;
using System.Linq;
using LangLang.Application.DTO;
using LangLang.Application.UseCases.User;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using static LangLang.Domain.Model.Exam;

namespace LangLang.Application.UseCases.Exam
{
    public class ExamAttendanceService : IExamAttendanceService
    {
        private readonly IExamService _examService;
        private readonly IStudentService _studentService;
        private readonly ITutorService _tutorService;
        private readonly IExamAttendanceRepository _examAttendanceRepository;

        public ExamAttendanceService(IExamService examService, IStudentService studentService, ITutorService tutorService, IExamAttendanceRepository examAttendanceRepository)
        {
            _examService = examService;
            _studentService = studentService;
            _tutorService = tutorService;
            _examAttendanceRepository = examAttendanceRepository;
        }

        public List<ExamAttendance> GetAttendancesForStudent(string studentId)
        {
            return _examAttendanceRepository.GetForStudent(studentId);
        }

        public List<ExamAttendance> GetAttendancesForExam(string examId)
        {
            return _examAttendanceRepository.GetForExam(examId);
        }

        public ExamAttendance? GetStudentAttendance(string studentId)
        {
            List<ExamAttendance> attendances = _examAttendanceRepository.GetForStudent(studentId);
            foreach (ExamAttendance attendance in attendances)
            {
                Domain.Model.Exam exam = _examService.GetExamById(attendance.ExamId)!;
                if (exam.ExamState != State.NotStarted && exam.ExamState != State.Graded && exam.ExamState != State.Reported)
                {
                    return attendance;
                }
            }
            return null;
        }
        public List<ExamAttendance> GetFinishedExamsStudent(string studentId)
        {
            List<ExamAttendance> allAttendances = _examAttendanceRepository.GetForStudent(studentId);
            List<ExamAttendance> finishedAttendances = new();
            foreach (ExamAttendance attendance in allAttendances)
            {
                Domain.Model.Exam exam = _examService.GetExamById(attendance.ExamId)!;
                if (exam.ExamState == State.Finished || exam.ExamState == State.Graded || exam.ExamState == State.Reported)
                    finishedAttendances.Add(attendance);
            }
            return finishedAttendances;
        }

        public ExamAttendance AddAttendance(string studentId, string examId)
        {
            ExamAttendance attendance = new ExamAttendance(examId, studentId, false, false);
            _examAttendanceRepository.Add(attendance);
            return attendance;
        }

        public void RemoveAttendee(string studentId, string examId)
        {
            var attendance = GetAttendance(studentId, examId);
            if (attendance == null) return;
            
            if(_examService.GetExamById(examId)!.ExamState != State.NotStarted)
            {
                _examService.GetExamById(examId)!.CancelAttendance();
            }
            _examAttendanceRepository.Delete(attendance.Id);
        }


        public void GradeStudent(string studentId, string examId, ExamGradeDto examGradeDto)
        {
            var attendance = GetAttendance(studentId, examId);
            if (attendance == null) return;

            attendance.Grade = new ExamGrade(
                examGradeDto.Reading,
                examGradeDto.Writing,
                examGradeDto.Listening,
                examGradeDto.Speaking
            );
            
            _examAttendanceRepository.Update(attendance.Id, attendance);
        }

        public void RateTutor(ExamAttendance attendance, int rating)
        {
            if (!attendance.isRated)
            {
                attendance.AddRating();
                Domain.Model.Exam exam = _examService.GetExamById(attendance.ExamId)!;
                //Tutor tutor = _tutorService.GetTutor(exam.TutorId);
                Tutor tutor = _tutorService.GetTutorForExam(exam.Id)!;
               _tutorService.AddRating(tutor, rating);  //after tutor id gets added to course/exam
                                                        //i will only pass tutor id and then the service will findById
            }
        }

        public List<Student> GetStudents(string examId)
        {
            return GetAttendancesForExam(examId)
                .Select(attendance => _studentService.GetStudentById(attendance.StudentId)!).ToList();
        }

        public ExamAttendance? GetAttendance(string studentId, string examId)
        {
            return _examAttendanceRepository.GetForStudent(studentId).FirstOrDefault(attendance => attendance.ExamId == examId);
        }

        public void AddPassedLanguagesToStudents(Domain.Model.Exam exam)
        {
            foreach (var attendance in GetAttendancesForExam(exam.Id))
            {
                if (attendance.Grade != null && attendance.Grade.IsPassing())
                {
                    var student = _studentService.GetStudentById(attendance.StudentId);
                    if(student == null) continue;
                    _studentService.AddPassedLanguage(student, exam.Language, exam.LanguageLevel);
                }
            }
        }

        public bool AvailableForApplication(Domain.Model.Exam exam, Student student)
        {
            var attendances = GetAttendancesForStudent(student.Id);
            foreach (var attendance in attendances)
            {
                var foundExam = _examService.GetExamById(attendance.ExamId);
                if (foundExam != null && foundExam.ExamState != State.Reported)
                    return false;
            }
            return true;
        }
    }
}
