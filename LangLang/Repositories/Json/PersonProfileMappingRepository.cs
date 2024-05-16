using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Repositories.Json;

public class PersonProfileMappingRepository : Repository<PersonProfileMapping>, IPersonProfileMappingRepository
{
    public PersonProfileMappingRepository(string filepath) : base(filepath)
    {
    }

    protected override string GetId(PersonProfileMapping mapping)
    {
        return mapping.Email;
    }
    
    public Dictionary<string, PersonProfileMapping> GetMap()
    {
        return Load();
    }
}