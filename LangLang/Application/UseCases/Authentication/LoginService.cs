using LangLang.Application.DTO;
using LangLang.Application.Stores;
using LangLang.Application.Utility.Authentication;
using LangLang.Domain.Model;

namespace LangLang.Application.UseCases.Authentication;

public class LoginService : ILoginService
{
    private readonly IAuthenticationStore _authenticationStore;
    private readonly IProfileService _profileService;
    private readonly IUserProfileMapper _userProfileMapper;

    public LoginService(IAuthenticationStore authenticationStore, IProfileService profileService, IUserProfileMapper userProfileMapper)
    {
        _authenticationStore = authenticationStore;
        _profileService = profileService;
        _userProfileMapper = userProfileMapper;
    }

    public LoginResult LogIn(string? email, string? password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return new LoginResult(false);

        Profile? profile = _profileService.GetProfile(email);

        if (profile == null) return new LoginResult(false);
        
        if (profile.Password != password)
            return new LoginResult(false, true);

        if (!profile.IsActive)
            return new LoginResult(false, true);
        
        _authenticationStore.CurrentUserProfile = profile;
        _authenticationStore.UserType = _userProfileMapper.GetPerson(profile).UserType;
        return new LoginResult(true, true, profile, _authenticationStore.UserType);
    }

    public void LogOut()
    {
        _authenticationStore.CurrentUserProfile = null;
        _authenticationStore.UserType = null;
    }
}