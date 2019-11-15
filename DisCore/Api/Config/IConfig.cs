using System.Threading.Tasks;

namespace DisCore.Api.Config
{
    public interface IConfig
    {
        Task Load();
        Task Save();

        Task<T> Get<T>(string key);
        Task Remove(string key);
        Task Set(string key, object val);
    }
}
