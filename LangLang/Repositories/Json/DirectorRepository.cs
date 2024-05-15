using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Repositories.Json
{
    public class DirectorRepository : AutoIdRepository<Director>, IDirectorRepository
    {
        public DirectorRepository(string filepath, string lastIdFilePath) : base(filepath, lastIdFilePath)
        {
        }
    }
}