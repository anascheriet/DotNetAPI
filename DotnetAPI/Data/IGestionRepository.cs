using System.Collections.Generic;
using System.Threading.Tasks;
using DotnetAPI.Model;

namespace DotnetAPI.Data
{
    public interface IGestionRepository
    {
         void Add<T> (T entity) where T:class;
         void Delete<T> (T entity) where T:class;
         Task<bool> Save();

//Class methods
        Task<IEnumerable<Class>> GetClasses(int userid);
        Task<Class> GetClass(int classid);
        Task<IEnumerable<AppUser>> GetClassMembers(int classid);
        Task<ClassAppUser> GetClassMemberRelation(int userid, int classid);
        Task<Class> GetClassByCode(string code);
//User methods
        Task<IEnumerable<AppUser>> GetUsers();
        Task<AppUser> GetUser(int userid);
    }
}