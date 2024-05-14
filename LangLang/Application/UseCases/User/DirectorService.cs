using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Application.UseCases.User
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