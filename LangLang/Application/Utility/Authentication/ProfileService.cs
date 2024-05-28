using System;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Application.Utility.Authentication;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepository;

    public ProfileService(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }
    
    public Profile? GetProfile(string email) => _profileRepository.Get(email);
    
    public bool IsEmailTaken(string email) => GetProfile(email) != null;

    public Profile AddProfile(Profile profile) => _profileRepository.Add(profile);

    public void DeactivateProfile(Profile profile)
    {
        profile.IsActive = false;
        _profileRepository.Update(profile.Email, profile);
    }

    public Profile UpdatePassword(Profile profile, string password)
    {
        profile.Password = password;
        profile = _profileRepository.Update(profile.Email, profile)
                  ?? throw new ArgumentException("No profile with the given id.");
        return profile;
    }

    public void DeleteProfile(string email) => _profileRepository.Delete(email);
}