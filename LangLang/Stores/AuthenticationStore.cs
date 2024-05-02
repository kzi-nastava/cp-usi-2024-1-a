using LangLang.DTO;
using LangLang.Model;
using LangLang.Services.AuthenticationServices;

namespace LangLang.Stores;

public class AuthenticationStore : IAuthenticationStore
{
    public Profile? CurrentUserProfile { get; set; }

    public bool IsLoggedIn => CurrentUserProfile != null;
    
    public UserType? UserType { get; set; }

    public UserDto CurrentUser => CurrentUserProfile != null ? userProfileMapper.GetPerson(CurrentUserProfile) : new UserDto(null, null);

    private readonly IUserProfileMapper userProfileMapper;

    public AuthenticationStore(IUserProfileMapper userProfileMapper)
    {
        this.userProfileMapper = userProfileMapper;
    }
}