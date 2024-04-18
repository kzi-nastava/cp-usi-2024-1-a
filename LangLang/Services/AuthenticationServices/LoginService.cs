using LangLang.DAO;
using LangLang.Model;
using LangLang.Stores;

namespace LangLang.Services.AuthenticationServices;

public class LoginService : ILoginService
{
    private readonly AuthenticationStore _authenticationStore;
    private readonly IStudentDAO _studentDao;
    private readonly ITutorDAO _tutorDao;
    private readonly IDirectorDAO _directorDao;

    public LoginService(AuthenticationStore authenticationStore, IStudentDAO studentDao, ITutorDAO tutorDao, IDirectorDAO directorDao)
    {
        _authenticationStore = authenticationStore;
        _studentDao = studentDao;
        _tutorDao = tutorDao;
        _directorDao = directorDao;
    }

    public LoginResult LogIn(string? email, string? password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return new LoginResult(false);

        LoginResult loginResult = LogInDirector(email, password);
        if (loginResult.IsValidUser) return loginResult;
        
        loginResult = LogInTutor(email, password);
        if (loginResult.IsValidUser) return loginResult;;
        
        return LogInStudent(email, password);
    }


    private LoginResult LogInStudent(string email, string password)
    {
        Student? student = _studentDao.GetStudent(email);

        if (student != null)
        {
            if (student.Password != password)
                return new LoginResult(false, true);
            _authenticationStore.CurrentUser = student;
            _authenticationStore.UserType = UserType.Student;
            return new LoginResult(true, true, student, UserType.Student);
        }

        return new LoginResult(false);
    }
    private LoginResult LogInTutor(string email, string password)
    {
        Tutor? tutor = _tutorDao.GetTutor(email);

        if (tutor != null)
        {
            if (tutor.Password != password)
                return new LoginResult(false, true);
            _authenticationStore.CurrentUser = tutor;
            _authenticationStore.UserType = UserType.Tutor;
            return new LoginResult(true, true, tutor, UserType.Tutor);
        }

        return new LoginResult(false);
    }
    private LoginResult LogInDirector(string email, string password)
    {
        Director? director = _directorDao.GetDirector(email);

        if (director != null)
        {
            if (director.Password != password)
                return new LoginResult(false, true);
            _authenticationStore.CurrentUser = director;
            _authenticationStore.UserType = UserType.Director;
            return new LoginResult(true, true, director, UserType.Director);
        }

        return new LoginResult(false);
    }

    public void LogOut()
    {
        _authenticationStore.CurrentUser = null;
        _authenticationStore.UserType = null;
    }
}