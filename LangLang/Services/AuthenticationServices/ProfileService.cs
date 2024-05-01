using LangLang.DAO;
using LangLang.Model;

namespace LangLang.Services.AuthenticationServices;

public class ProfileService : IProfileService
{
    private readonly IProfileDAO _profileDao;

    public ProfileService(IProfileDAO profileDao)
    {
        _profileDao = profileDao;
    }
    
    public Profile? GetProfile(string email) => _profileDao.GetProfile(email);
    
    public bool IsEmailTaken(string email) => GetProfile(email) == null;

    public Profile AddProfile(Profile profile) => _profileDao.AddProfile(profile);

    public void DeactivateProfile(Profile profile)
    {
        profile.IsActive = false;
        _profileDao.UpdateProfile(profile.Email, profile);
    }
}