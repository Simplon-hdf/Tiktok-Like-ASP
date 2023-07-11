using Microsoft.EntityFrameworkCore;
using TiktokLikeASP.Models;

namespace TiktokLikeASP.Context
{
    public class ApplicationDbContext:DbContext
    {
        protected readonly IConfiguration Configuration;

        public ApplicationDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasMany(e => e.UserLikes)
                .WithMany(e => e.LikedPosts)
                .UsingEntity(
                    "UserLikesPost",
                    l => l.HasOne(typeof(User)).WithMany().HasForeignKey("UsersId").HasPrincipalKey(nameof(User.Id)),
                    r => r.HasOne(typeof(Post)).WithMany().HasForeignKey("PostsId").HasPrincipalKey(nameof(Post.Id)),
                    j => j.HasKey("PostsId", "UsersId"));

            modelBuilder.Entity<Post>()
                .HasOne(e => e.Creator)
                .WithMany(e => e.PublishedPosts)
                .HasForeignKey(e => e.Id)
                .IsRequired();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }

    }
}
