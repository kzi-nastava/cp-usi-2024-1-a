using System;
using System.Collections.Generic;
using LangLang.Core;
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
        return new Dictionary<string, PersonProfileMapping>(Load());
    }

    public new PersonProfileMapping Add(PersonProfileMapping mapping)
    {
        mapping = base.Add(mapping);
        NotifyObservers(mapping);
        return mapping;
    }

    private readonly HashSet<IObserver<PersonProfileMapping>> _observers = new();
    
    public IDisposable Subscribe(IObserver<PersonProfileMapping> observer)
    {
        _observers.Add(observer);
        return new Unsubscriber<PersonProfileMapping>(_observers, observer);
    }

    private void NotifyObservers(PersonProfileMapping mapping)
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(mapping);
        }
    }
}