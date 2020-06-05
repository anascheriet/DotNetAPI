using System.Threading.Tasks;

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
    }
}