using System;
using System.Collections.Generic;
using LangLang.Domain.Model;

namespace LangLang.Domain.RepositoryInterfaces;

public interface IPersonProfileMappingDAO : IObservable<PersonProfileMapping>
{
    public Dictionary<string, PersonProfileMapping> GetAll();
    public void AddMapping(PersonProfileMapping mapping);
}