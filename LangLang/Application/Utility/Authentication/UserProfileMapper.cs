using System;
using System.Collections.Generic;
using LangLang.Application.DTO;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using UserType = LangLang.Domain.Model.UserType;

namespace LangLang.Application.Utility.Authentication;

public class UserProfileMapper : IUserProfileMapper
{
    private readonly Dictionary<string, PersonProfileMapping> profileToUser;
    private readonly Dictionary<string, string> studentToProfile = new();
    private readonly Dictionary<string, string> tutorToProfile = new();
    private readonly Dictionary<string, string> directorToProfile = new();

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
        
        profileToUser = personProfileMappingRepository.GetMap();
        InitPersonToProfileMappings();
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
            UserType.Student => studentToProfile.GetValueOrDefault(((Student)user.Person).Id),
            UserType.Tutor => tutorToProfile.GetValueOrDefault(((Tutor)user.Person).Id),
            UserType.Director => directorToProfile.GetValueOrDefault(((Director)user.Person).Id),
            _ => throw new ArgumentException("User type not supported.")
        };

        return email == null ? null : _profileService.GetProfile(email);
    }
}