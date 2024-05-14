using System;
using System.Collections.Generic;
using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;
using LangLang.Repositories.Json.Util;

namespace LangLang.Repositories.Json;

public class PersonProfileMappingDAO : IPersonProfileMappingDAO
{
    private Dictionary<string, PersonProfileMapping>? _mappings;
    
    private Dictionary<string, PersonProfileMapping> Mappings
    {
        get
        {
            _mappings ??= JsonUtil.ReadFromFile<PersonProfileMapping>(Constants.PersonProfileMappingFilePath);
            return _mappings!;
        }
        set => _mappings = value;
    }

    public Dictionary<string, PersonProfileMapping> GetAll()
    {
        return Mappings;
    }

    public void AddMapping(PersonProfileMapping mapping)
    {
        Mappings.Add(mapping.Email, mapping);
        SaveMappings();
    }
    
    private void SaveMappings()
    {
        JsonUtil.WriteToFile(Mappings, Constants.PersonProfileMappingFilePath);
    }


    private readonly HashSet<IObserver<PersonProfileMapping>> _observers = new();
    
    public IDisposable Subscribe(IObserver<PersonProfileMapping> observer)
    {
        _observers.Add(observer);
        return new Unsubscriber<PersonProfileMapping>(_observers, observer);
    }
}