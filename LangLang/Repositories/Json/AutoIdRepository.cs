using System.IO;
using LangLang.Domain.Model;

namespace LangLang.Repositories.Json;

public class AutoIdRepository<T> : Repository<T> where T : class, IEntity
{
    private readonly string _lastIdFilePath;

    public AutoIdRepository(string filepath, string lastIdFilePath) : base(filepath)
    {
        _lastIdFilePath = lastIdFilePath;
    }

    protected override string GetId(T _)
    {
        return Read().ToString();
    }

    public new T Add(T item)
    {
        var nextId = Read();
        nextId++;
        Write(nextId);
        item.Id = nextId.ToString();
        return base.Add(item);
    }

    private void Write(int nextId)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_lastIdFilePath)!);
        using var writer = new StreamWriter(_lastIdFilePath);
        writer.WriteLine(nextId);
    }

    private int Read()
    {
        if (!File.Exists(_lastIdFilePath)) return 0;
        using var reader = new StreamReader(_lastIdFilePath);
        return int.Parse(reader.ReadLine() ?? "0");
    }
}