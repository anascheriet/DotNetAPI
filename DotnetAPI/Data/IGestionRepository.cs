using System.Threading.Tasks;

namespace DotnetAPI.Data
{
    public interface IGestionRepository
    {
         void Add<T> (T entity) where T:class;
         void Delete<T> (T entity) where T:class;
         Task<bool> Save();
    }
}