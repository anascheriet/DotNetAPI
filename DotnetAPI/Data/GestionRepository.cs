using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data
{
    public class GestionRepository : IGestionRepository
    {
        private readonly AppDbContext _db;

        public GestionRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add<T>(T entity) where T : class
        {
            _db.Add(entity);
        }

        public async Task<bool> Save()
        {
            return (await _db.SaveChangesAsync() > 0);
        }

        public void Delete<T>(T entity) where T : class
        {
            _db.Remove(entity);
        }

        //Class Gets
        public async Task<IEnumerable<Class>> GetClasses(int userid)
        {
            var verifiedMembers = _db.ClassMembers.Where(cm => cm.verified == true);

            var query = from cm in verifiedMembers
                        join cl in _db.classes
                        on cm.ClassId equals cl.ClassId
                        join usr in _db.Users
                        on cm.MemberId equals usr.Id
                        where cm.MemberId == userid
                        select cl;
            return await query.ToListAsync();
        }
        public async Task<Class> GetClass(int classid)
        {
            return await _db.classes.FirstOrDefaultAsync(c => c.ClassId == classid);
        }
        public async Task<Class> GetClassByCode(string code)
        {
            return await _db.classes.FirstOrDefaultAsync(a => a.InvitationCode == code);
        }

        public async Task<IEnumerable<AppUser>> GetUsers()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<AppUser> GetUser(int userid)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == userid);
        }

        public async Task<IEnumerable<AppUser>> GetClassMembers(int classid)
        {
            var verifiedMembers = _db.ClassMembers.Where(cm => cm.ClassId == classid && cm.verified == true);
            var query = from vm in verifiedMembers
                        join usr in _db.Users
                        on vm.MemberId equals usr.Id
                        where vm.ClassId == classid
                        select usr;
            return await query.ToListAsync();

        }
        public async Task<IEnumerable<AppUser>> GetPendingMembers(int classid)
        {
            var pendingMembers = _db.ClassMembers.Where(cm => cm.ClassId == classid && cm.verified == false);
            var query = from pm in pendingMembers
                        join usr in _db.Users
                        on pm.MemberId equals usr.Id
                        where pm.ClassId == classid
                        select usr;
            return await query.ToListAsync();


        }
        public async Task<ClassAppUser> GetClassMemberRelation(int userid, int classid)
        {
            return await _db.ClassMembers.FirstOrDefaultAsync(cm => cm.MemberId == userid && cm.ClassId == classid);
        }

        public async Task<IEnumerable<Publication>> GetPublications(int classid)
        {
            return await _db.publications.Where(p => p.ClassId == classid).ToListAsync();
        }

        public async Task<Publication> GetPublication(int pubid)
        {
            return await _db.publications.FirstOrDefaultAsync(pub => pub.PublicationId == pubid);
        }

        public async Task<IEnumerable<Attachment>> GetAttachments(int pubid)
        {
            return await _db.attachements.Where(at => at.PublicationId == pubid).ToListAsync();
        }

        public async Task<Attachment> GetAttachment(int attachementid)
        {
            return await _db.attachements.FirstOrDefaultAsync(at => at.AttachmentId == attachementid);
        }

        public async Task<IEnumerable<Comment>> GetComments(int pubid)
        {
            return await _db.comments.Where(com => com.PublicationId == pubid).ToListAsync();
        }

        public async Task<Comment> GetComment(int Commentid)
        {
            return await _db.comments.FirstOrDefaultAsync(com => com.CommentId == Commentid);
        }


    }
}