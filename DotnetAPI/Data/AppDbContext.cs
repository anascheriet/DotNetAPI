using DotnetAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data
{
    public class AppDbContext : IdentityDbContext<AppUser,AppRole,int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
               
        }
        protected override void OnModelCreating(ModelBuilder builder){
            base.OnModelCreating(builder);
              builder.Entity<ClassPending>().HasKey(k => new{k.ClassId, k.AppUserId});
        }
        public DbSet<Class> classes { get; set; }
        public DbSet<Publication> publications { get; set; }
        public DbSet<Attachement> attachements { get; set; }
        public DbSet<Comment> comments { get; set; }
        public DbSet<ClassPending> classpendings { get; set; }
    }
}
