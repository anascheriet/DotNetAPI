using DotnetAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ClassAppUser>().HasKey(k => new { k.MemberId, k.ClassId });

            /*
                builder.Entity<Publication>().Property(pub => pub.PublicationId).ValueGeneratedOnAdd();
                builder.Entity<Attachment>().Property(at => at.AttachmentId).ValueGeneratedOnAdd();
                builder.Entity<Comment>().Property(com => com.CommentId).ValueGeneratedOnAdd();
            */
            //Many To Many (Classes with Users)
            builder.Entity<ClassAppUser>().HasOne(cu => cu.Class).WithMany(c => c.ClassMembers).HasForeignKey(cu => cu.ClassId);
            builder.Entity<ClassAppUser>().HasOne(cu => cu.Member).WithMany(u => u.ClassMembers).HasForeignKey(cu => cu.MemberId);
            //Many To One (Classpublications with publication)
            builder.Entity<Class>().HasMany(c => c.Publications).WithOne(p => p.Class).HasForeignKey(k => k.ClassId);
            //Many To ONe (PublicationAttachments with Publication)
            builder.Entity<Publication>().HasMany(p => p.Attachements).WithOne(a => a.Publication).HasForeignKey(k => k.PublicationId);
            //Many To ONe (PublicationComments with Publication)
            builder.Entity<Publication>().HasMany(p => p.Comments).WithOne(c => c.Publication).HasForeignKey(k => k.PublicationId);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseLazyLoadingProxies();

        }
        public DbSet<Class> classes { get; set; }
        public DbSet<ClassAppUser> ClassMembers { get; set; }
        public DbSet<Publication> publications { get; set; }
        public DbSet<Attachment> attachements { get; set; }
        public DbSet<Comment> comments { get; set; }

    }
}
