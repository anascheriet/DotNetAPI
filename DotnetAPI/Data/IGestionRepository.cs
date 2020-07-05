using System.Collections.Generic;
using System.Threading.Tasks;
using DotnetAPI.Model;

namespace DotnetAPI.Data
{
    public interface IGestionRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> Save();

        //Class methods
        Task<IEnumerable<Class>> GetClasses(int userid);
        Task<Class> GetClass(int classid);
        Task<IEnumerable<AppUser>> GetClassMembers(int classid);
        Task<IEnumerable<AppUser>> GetPendingMembers(int classid);
        Task<ClassAppUser> GetClassMemberRelation(int userid, int classid);
        Task<Class> GetClassByCode(string code);
        //User methods
        Task<IEnumerable<AppUser>> GetUsers();
        Task<AppUser> GetUser(int userid);
        //publication methods

        Task<IEnumerable<Publication>> GetPublications(int classid);
        Task<Publication> GetPublication(int pubid);
        //Attachment methods
        Task<IEnumerable<Attachment>> GetAttachments(int pubid);
        Task<Attachment> GetAttachment(int attachementid);
        //Comment Methods
        Task<IEnumerable<Comment>> GetComments(int pubid);
        Task<Comment> GetComment(int Commentid);

    }
}