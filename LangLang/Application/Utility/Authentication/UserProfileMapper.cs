using System;
using System.Collections.Generic;
using LangLang.Application.DTO;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using UserType = LangLang.Domain.Model.UserType;

namespace LangLang.Application.Utility.Authentication;

public class UserProfileMapper : IUserProfileMapper, IObserver<PersonProfileMapping>
{
    private readonly Dictionary<string, PersonProfileMapping> _profileToUser;
    private readonly Dictionary<string, string> _studentToProfile = new();
    private readonly Dictionary<string, string> _tutorToProfile = new();
    private readonly Dictionary<string, string> _directorToProfile = new();

    private readonly IProfileService _profileService;
    private readonly IStudentRepository _studentRepository;
    private readonly ITutorRepository _tutorRepository;
    private readonly IDirectorRepository _directorRepository;

    public UserProfileMapper(IPersonProfileMappingRepository personProfileMappingRepository, IProfileService profileService, IStudentRepository studentRepository, ITutorRepository tutorRepository, IDirectorRepository directorRepository)
    {
        _profileService = profileService;
        _studentRepository = studentRepository;
        _tutorRepository = tutorRepository;
        _directorRepository = directorRepository;
        
        _profileToUser = personProfileMappingRepository.GetMap();
        InitPersonToProfileMappings();
        personProfileMappingRepository.Subscribe(this);
    }

    private void InitPersonToProfileMappings()
    {
        foreach (var mapping in _profileToUser.Values)
        {
            var mapper = mapping.UserType switch
            {
                UserType.Student => _studentToProfile,
                UserType.Tutor => _tutorToProfile,
                UserType.Director => _directorToProfile,
                _ => throw new ArgumentException("User type not supported.")
            };
            mapper.Add(mapping.UserId, mapping.Email);
        }
    }

    public UserDto GetPerson(Profile profile)
    {
        var mapping = _profileToUser[profile.Email];
        Person? person = mapping.UserType switch
        {
            UserType.Student => _studentRepository.Get(mapping.UserId),
            UserType.Tutor => _tutorRepository.Get(mapping.UserId),
            UserType.Director => _directorRepository.Get(mapping.UserId),
            _ => null
        };
        return new UserDto(person, mapping.UserType);
    }

    public Profile? GetProfile(UserDto user)
    {
        if (user.Person == null) return null;
        
        var email = user.UserType switch
        {
            UserType.Student => _studentToProfile.GetValueOrDefault(((Student)user.Person).Id),
            UserType.Tutor => _tutorToProfile.GetValueOrDefault(((Tutor)user.Person).Id),
            UserType.Director => _directorToProfile.GetValueOrDefault(((Director)user.Person).Id),
            _ => throw new ArgumentException("User type not supported.")
        };

        return email == null ? null : _profileService.GetProfile(email);
    }

    public void OnCompleted()
    {
        _profileToUser.Clear();
        _studentToProfile.Clear();
        _tutorToProfile.Clear();
        _directorToProfile.Clear();
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(PersonProfileMapping mapping)
    {
        _profileToUser.Add(mapping.Email, mapping);
        var mapper = mapping.UserType switch
        {
            UserType.Student => _studentToProfile,
            UserType.Tutor => _tutorToProfile,
            UserType.Director => _directorToProfile,
            _ => throw new ArgumentException("User type not supported.")
        };
        mapper.Add(mapping.UserId, mapping.Email);
    }
}