using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Repositories.Json
{
    public class TutorRepository : AutoIdRepository<Tutor>, ITutorRepository
    {
        public TutorRepository(string filepath, string lastIdFilePath) : base(filepath, lastIdFilePath)
        {
        }

        public bool Exists(string id) => GetAll().ContainsKey(id);
    }
}