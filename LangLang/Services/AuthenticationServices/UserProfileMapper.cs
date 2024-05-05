using System;
using System.Collections.Generic;
using LangLang.DAO;
using LangLang.DTO;
using LangLang.Model;
using UserType = LangLang.Model.UserType;

namespace LangLang.Services.AuthenticationServices;

public class UserProfileMapper : IUserProfileMapper, IObserver<PersonProfileMapping>
{
    private readonly Dictionary<string, PersonProfileMapping> profileToUser;
    private readonly Dictionary<string, string> studentToProfile = new();
    private readonly Dictionary<string, string> tutorToProfile = new();
    private readonly Dictionary<string, string> directorToProfile = new();

    private readonly IProfileService _profileService;
    private readonly IStudentDAO _studentDao;
    private readonly ITutorDAO _tutorDao;
    private readonly IDirectorDAO _directorDao;

    public UserProfileMapper(IPersonProfileMappingDAO personProfileMappingDao, IProfileService profileService, IStudentDAO studentDao, ITutorDAO tutorDao, IDirectorDAO directorDao)
    {
        _profileService = profileService;
        _studentDao = studentDao;
        _tutorDao = tutorDao;
        _directorDao = directorDao;
        
        profileToUser = personProfileMappingDao.GetAll();
        InitPersonToProfileMappings();
        personProfileMappingDao.Subscribe(this);
    }

    private void InitPersonToProfileMappings()
    {
        foreach (var mapping in profileToUser.Values)
        {
            var mapper = mapping.UserType switch
            {
                UserType.Student => studentToProfile,
                UserType.Tutor => tutorToProfile,
                UserType.Director => directorToProfile,
                _ => throw new ArgumentException("User type not supported.")
            };
            mapper.Add(mapping.UserId, mapping.Email);
        }
    }

    public UserDto GetPerson(Profile profile)
    {
        var mapping = profileToUser[profile.Email];
        Person? person = mapping.UserType switch
        {
            UserType.Student => _studentDao.GetStudent(mapping.UserId),
            UserType.Tutor => _tutorDao.GetTutor(mapping.UserId),
            UserType.Director => _directorDao.GetDirector(mapping.UserId),
            _ => null
        };
        return new UserDto(person, mapping.UserType);
    }

    public Profile? GetProfile(UserDto user)
    {
        if (user.Person == null) return null;
        
        var email = user.UserType switch
        {
            UserType.Student => studentToProfile.GetValueOrDefault(((Student)user.Person).Id),
            UserType.Tutor => tutorToProfile.GetValueOrDefault(((Tutor)user.Person).Id),
            UserType.Director => directorToProfile.GetValueOrDefault(((Director)user.Person).Id),
            _ => throw new ArgumentException("User type not supported.")
        };

        return email == null ? null : _profileService.GetProfile(email);
    }

    public void OnCompleted()
    {
        profileToUser.Clear();
        studentToProfile.Clear();
        tutorToProfile.Clear();
        directorToProfile.Clear();
    }

    public void OnError(Exception error)
    {
        
    }

    public void OnNext(PersonProfileMapping mapping)
    {
        profileToUser.Add(mapping.Email, mapping);
        var mapper = mapping.UserType switch
        {
            UserType.Student => studentToProfile,
            UserType.Tutor => tutorToProfile,
            UserType.Director => directorToProfile,
            _ => throw new ArgumentException("User type not supported.")
        };
        mapper.Add(mapping.UserId, mapping.Email);
    }
}