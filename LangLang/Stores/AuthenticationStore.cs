using LangLang.Model;
using LangLang.Services.AuthenticationServices;

namespace LangLang.Stores;

public class AuthenticationStore
{
    public User? CurrentUser { get; set; }

    public bool IsLoggedIn => CurrentUser != null;
    
    public UserType? UserType { get; set; }
}