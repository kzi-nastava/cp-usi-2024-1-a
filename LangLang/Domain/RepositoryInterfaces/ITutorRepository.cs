using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface ITutorRepository : IRepository<Tutor>
{
    public bool Exists(string id);
}