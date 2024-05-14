using LangLang.Application.DTO;
using LangLang.Application.Utility.Authentication;
using LangLang.Domain.Model;

namespace LangLang.Application.Stores;

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