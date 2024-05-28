using System;
using System.Collections.Generic;
using LangLang.Application.DTO;
using LangLang.Application.UseCases.Course;
using LangLang.Application.UseCases.Exam;
using LangLang.Application.Utility.Authentication;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Application.UseCases.User
{
    public class AccountService : IAccountService
    {
        private readonly IProfileService _profileService;
        private readonly IStudentService _studentService;
        private readonly ITutorService _tutorService;
        private readonly IStudentCourseCoordinator _studentCourseCoordinator;
        private readonly IExamCoordinator _examCoordinator;
        private readonly IPersonProfileMappingRepository _personProfileMappingRepository;
        private readonly IUserProfileMapper _userProfileMapper;
        private readonly ICourseService _courseService;

        public AccountService(IProfileService profileService, IStudentService studentService, ITutorService tutorService, IStudentCourseCoordinator studentCourseCoordinator, IPersonProfileMappingRepository personProfileMappingRepository, IUserProfileMapper userProfileMapper, IExamCoordinator examCoordinator, ICourseService courseService)
        {
            _profileService = profileService;
            _studentService = studentService;
            _tutorService = tutorService;
            _studentCourseCoordinator = studentCourseCoordinator;
            _personProfileMappingRepository = personProfileMappingRepository;
            _userProfileMapper = userProfileMapper;
            _examCoordinator = examCoordinator;
            _courseService = courseService;
        }

        public string GetEmailByUserId(string userId, UserType userType)
        {
            return _personProfileMappingRepository.GetEmailByUserId(userId, userType);
        }

        public void UpdateStudent(string studentId, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber)
        {
            if (_studentCourseCoordinator.GetStudentAttendingCourse(studentId) != null)
            {
                throw new ArgumentException("Student applied for courses, editing profile not allowed");
            }
            if (_examCoordinator.GetAttendingExam(studentId) != null)
            {
                throw new ArgumentException("Student applied for exam, editing profile not allowed");
            }

            Student student = _studentService.GetStudentById(studentId)!;
            _studentService.UpdateStudent(student, name, surname, birthDate, gender, phoneNumber);

            Profile profile = _userProfileMapper.GetProfile(new UserDto(student, UserType.Student))
                              ?? throw new InvalidOperationException("No profile associated with student.");
            _profileService.UpdatePassword(profile, password);
        }

        public void DeleteStudent(Student student)
        {
            _studentCourseCoordinator.RemoveAttendee(student.Id);
            _examCoordinator.RemoveAttendee(student.Id);
            _studentService.DeleteAccount(student);
            var profile = _userProfileMapper.GetProfile(new UserDto(student, UserType.Student));
            if (profile != null)
            {
                _profileService.DeleteProfile(profile.Email);
                _personProfileMappingRepository.Delete(profile.Email);
            }
        }

        public void DeactivateStudentAccount(Student student)
        {
            _studentCourseCoordinator.RemoveAttendee(student.Id);
            _examCoordinator.RemoveAttendee(student.Id);
            var profile = _userProfileMapper.GetProfile(new UserDto(student, UserType.Student));
            if(profile != null)
                _profileService.DeactivateProfile(profile);
        }

        public void RegisterStudent(RegisterStudentDto registerDto)
        {
            var profile = _profileService.AddProfile(new Profile(
                registerDto.Email,
                registerDto.Password
            ));
            var student = _studentService.AddStudent(new Student(
                registerDto.Name,
                registerDto.Surname,
                registerDto.BirthDay,
                registerDto.Gender,
                registerDto.PhoneNumber,
                registerDto.EducationLevel,
                0
            ));
            _personProfileMappingRepository.Add(new PersonProfileMapping(
                profile.Email,
                UserType.Student,
                student.Id
            ));
        }

        public Tutor RegisterTutor(RegisterTutorDto registerDto)
        {
            var profile = _profileService.AddProfile(new Profile(
                registerDto.Email,
                registerDto.Password
            ));
            var tutor = _tutorService.AddTutor(new Tutor(
                registerDto.Name,
                registerDto.Surname,
                registerDto.BirthDay,
                registerDto.Gender,
                registerDto.PhoneNumber,
                registerDto.KnownLanguages,
                new int[10],
                registerDto.DateAdded
            ));
            _personProfileMappingRepository.Add(new PersonProfileMapping(
                profile.Email,
                UserType.Tutor,
                tutor.Id
            ));
            return tutor;
        }
        public Tutor UpdateTutor(string tutorId, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber, List<Tuple<Language, LanguageLevel>> knownLanguages, DateTime dateAdded)
        {
            Tutor tutor = _tutorService.GetTutorById(tutorId)!;
            _tutorService.UpdateTutor(tutor, name, surname, birthDate, gender, phoneNumber, knownLanguages, dateAdded);

            Profile profile = _userProfileMapper.GetProfile(new UserDto(tutor, UserType.Tutor))
                              ?? throw new InvalidOperationException("No profile associated with tutor.");
            _profileService.UpdatePassword(profile, password);
            return tutor;
        }

        public void DeleteTutor(Tutor tutor)
        {
            _examCoordinator.DeleteExamsByTutor(tutor);
            _studentCourseCoordinator.RemoveCoursesOfTutor(tutor);
            _courseService.RemoveTutorFromAllCourses(tutor);
            _tutorService.DeleteAccount(tutor);
            var profile = _userProfileMapper.GetProfile(new UserDto(tutor, UserType.Tutor));
            if (profile != null)
            {
                _profileService.DeleteProfile(profile.Email);
                _personProfileMappingRepository.Delete(profile.Email);
            }
        }
    }
}
