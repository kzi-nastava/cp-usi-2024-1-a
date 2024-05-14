using LangLang.Application.DTO;
using LangLang.Domain.Model;

namespace LangLang.Application.Stores;

public interface IAuthenticationStore
{
    public Profile? CurrentUserProfile { get; set; }
    public bool IsLoggedIn { get; }
    public UserType? UserType { get; set; }
    public UserDto CurrentUser { get; }
}