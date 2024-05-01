using LangLang.DTO;
using LangLang.Model;

namespace LangLang.Stores;

public interface IAuthenticationStore
{
    public Profile? CurrentUserProfile { get; set; }
    public bool IsLoggedIn { get; }
    public UserType? UserType { get; set; }
    public UserDto CurrentUser { get; }
}