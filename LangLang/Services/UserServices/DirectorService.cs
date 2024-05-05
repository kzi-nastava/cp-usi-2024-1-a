using LangLang.DAO;

namespace LangLang.Services.UserServices
{
    public class DirectorService : IDirectorService
    {
        private readonly IDirectorDAO _directorDao;

        public DirectorService(IDirectorDAO directorDao)
        {
            _directorDao = directorDao;
        }
    }
}