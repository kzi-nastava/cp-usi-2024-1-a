using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Repositories.Json;

public class StudentRepository : AutoIdRepository<Student>, IStudentRepository
{
    public StudentRepository(string filepath, string lastIdFilePath) : base(filepath, lastIdFilePath)
    {
    }
}