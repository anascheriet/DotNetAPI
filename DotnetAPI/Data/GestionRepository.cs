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
            var query = from cm in _db.ClassMembers
            join cl in _db.classes
            on cm.ClassId equals cl.ClassId
            join usr in _db.Users
            on cm.AppUserId equals usr.Id
            where cm.AppUserId == userid
            select cl;
            return await query.ToListAsync();
        }
        public async Task<Class> GetClass(int classid)
        {
            return  await _db.classes.FirstOrDefaultAsync(c => c.ClassId == classid);
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
            var query = from cm in _db.ClassMembers
            join cl in _db.classes
            on cm.ClassId equals cl.ClassId
            join usr in _db.Users
            on cm.AppUserId equals usr.Id
            where cm.ClassId == classid
            select usr;
            return await query.ToListAsync();

        }

        public async Task<ClassAppUser> GetClassMemberRelation(int userid, int classid)
        {
            return await _db.ClassMembers.FirstOrDefaultAsync(cm => cm.AppUserId == userid && cm.ClassId == classid);
        }
    }
}