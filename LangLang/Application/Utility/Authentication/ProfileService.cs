using System;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Application.Utility.Authentication;

public class ProfileService : IProfileService
{
    private readonly IProfileDAO _profileDao;

    public ProfileService(IProfileDAO profileDao)
    {
        _profileDao = profileDao;
    }
    
    public Profile? GetProfile(string email) => _profileDao.GetProfile(email);
    
    public bool IsEmailTaken(string email) => GetProfile(email) != null;

    public Profile AddProfile(Profile profile) => _profileDao.AddProfile(profile);

    public void DeactivateProfile(Profile profile)
    {
        profile.IsActive = false;
        _profileDao.UpdateProfile(profile.Email, profile);
    }

    public Profile UpdatePassword(Profile profile, string password)
    {
        profile.Password = password;
        profile = _profileDao.UpdateProfile(profile.Email, profile)
                  ?? throw new ArgumentException("No profile with the given id.");
        return profile;
    }

    public void DeleteProfile(string email) => _profileDao.DeleteProfile(email);
}