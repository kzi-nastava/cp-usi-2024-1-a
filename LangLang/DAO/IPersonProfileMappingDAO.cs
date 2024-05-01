using System;
using System.Collections.Generic;
using LangLang.Model;

namespace LangLang.DAO;

public interface IPersonProfileMappingDAO : IObservable<PersonProfileMapping>
{
    public Dictionary<string, PersonProfileMapping> GetAll();
    public void AddMapping(PersonProfileMapping mapping);
}