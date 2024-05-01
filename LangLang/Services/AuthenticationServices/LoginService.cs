using LangLang.DAO;
using LangLang.DTO;
using LangLang.Model;
using LangLang.Stores;

namespace LangLang.Services.AuthenticationServices;

public class LoginService : ILoginService
{
    private readonly IAuthenticationStore _authenticationStore;
    private readonly IProfileDAO _profileDao;
    private readonly IUserProfileMapper _userProfileMapper;

    public LoginService(IAuthenticationStore authenticationStore, IProfileDAO profileDao, IUserProfileMapper userProfileMapper)
    {
        _authenticationStore = authenticationStore;
        _profileDao = profileDao;
        _userProfileMapper = userProfileMapper;
    }

    public LoginResult LogIn(string? email, string? password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return new LoginResult(false);

        Profile? profile = _profileDao.GetProfileById(email);

        if (profile == null) return new LoginResult(false);
        
        if (profile.Password != password)
            return new LoginResult(false, true);
        
        _authenticationStore.CurrentUserProfile = profile;
        _authenticationStore.UserType = _userProfileMapper.GetPerson(profile).UserType;
        return new LoginResult(true, true, profile, UserType.Student);
    }

    public void LogOut()
    {
        _authenticationStore.CurrentUserProfile = null;
        _authenticationStore.UserType = null;
    }
}