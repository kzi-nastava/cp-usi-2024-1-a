using System.Collections.Generic;
using LangLang.Repositories.Json.Util;

namespace LangLang.Repositories.Json;

public abstract class Repository<T> where T : class
{
    private readonly string _filepath;

    protected Repository(string filepath)
    {
        _filepath = filepath;
    }

    protected abstract string GetId(T item);

    public Dictionary<string, T> GetAll()
    {
        return Load();
    }
    
    public T? Get(string id)
    {
        return Load().GetValueOrDefault(id);
    }
    
    public List<T> Get(List<string> ids)
    {
        var items = Load();
        List<T> resultItems = new();
        foreach (var id in ids)
        {
            if (items.TryGetValue(id, out var item))
            {
                resultItems.Add(item);
            }
        }
        return resultItems;
    }

    public T Add(T item)
    {
        var items = Load();
        items.Add(GetId(item), item);
        Save(items);
        return item;
    }

    public T? Update(string id, T exam)
    {
        var items = Load();
        if (!items.ContainsKey(id)) return null;
        items[id] = exam;
        Save(items);
        return exam;
    }

    public void Delete(string id)
    {
        var items = Load();
        items.Remove(id);
        Save(items);
    }
    
    protected Dictionary<string, T> Load()
    {
        return JsonUtil.ReadFromFile<T>(_filepath);
    }

    protected void Save(Dictionary<string, T> items)
    {
        JsonUtil.WriteToFile(items, _filepath);
    }
}