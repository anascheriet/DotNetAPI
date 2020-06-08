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

              builder.Entity<ClassAppUser>().HasKey(k => new{k.AppUserId, k.ClassId});

              builder.Entity<ClassAppUser>().HasOne(cu => cu.Class).WithMany(c => c.ClassMembers).HasForeignKey(cu => cu.ClassId);
              builder.Entity<ClassAppUser>().HasOne(cu => cu.AppUser).WithMany(u => u.ClassMembers).HasForeignKey(cu => cu.AppUserId);
              builder.Entity<Class>().HasMany(c => c.publications).WithOne(p => p.Class).HasForeignKey(k => k.PublicationId);
              builder.Entity<Publication>().HasMany(p => p.Attachements).WithOne(a => a.Publication).HasForeignKey(k => k.AttachementId);
              builder.Entity<Publication>().HasMany(p => p.Comments).WithOne(c => c.Publication).HasForeignKey(k => k.CommentId);
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseLazyLoadingProxies();

        }
        public DbSet<Class> classes { get; set; }
        public DbSet<ClassAppUser> ClassMembers { get; set; }
        public DbSet<Publication> publications { get; set; }
        public DbSet<Attachement> attachements { get; set; }
        public DbSet<Comment> comments { get; set; }
        
    }
}
