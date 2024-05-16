using System.Collections.Generic;

namespace LangLang.Domain.RepositoryInterfaces;

public interface IRepository<T> where T : class
{
    public List<T> GetAll();
    public T? Get(string id);
    public List<T> Get(List<string> ids);
    public T Add(T exam);
    public T? Update(string id, T exam);    
    public void Delete(string id);
}