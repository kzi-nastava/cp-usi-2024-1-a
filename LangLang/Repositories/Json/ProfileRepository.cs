using LangLang.Domain.Model;
using LangLang.Domain.RepositoryInterfaces;

namespace LangLang.Repositories.Json;

public class ProfileRepository : Repository<Profile>, IProfileRepository
{
    public ProfileRepository(string filepath) : base(filepath)
    {
    }

    protected override string GetId(Profile profile)
    {
        return profile.Email;
    }
}