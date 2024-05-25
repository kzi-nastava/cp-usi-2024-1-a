using System;
using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface IPersonProfileMappingRepository : IRepository<PersonProfileMapping>, IObservable<PersonProfileMapping>
{
    public Dictionary<string, PersonProfileMapping> GetMap();
    public string GetEmailByUserId(string userId);
}