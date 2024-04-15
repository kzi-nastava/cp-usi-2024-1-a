using LangLang.DAO;

namespace LangLang.Services.UserServices
{
    public class DirectorService : IDirectorService
    {
        DirectorDAO directorDAO = DirectorDAO.GetInstance();
    }
}