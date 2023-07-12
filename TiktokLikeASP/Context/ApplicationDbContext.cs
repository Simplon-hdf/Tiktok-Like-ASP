using Microsoft.EntityFrameworkCore;
using Npgsql;
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
            var conStrBuilder = new NpgsqlConnectionStringBuilder(Configuration.GetConnectionString("WebApiDatabase"));
            conStrBuilder.Password = Configuration["DB_PASSWORD"];
            conStrBuilder.Username = Configuration["DB_USER"];
            conStrBuilder.Database = Configuration["DB_NAME"];

            // connect to postgres with connection string from app settings
            options.UseNpgsql(conStrBuilder.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasMany(e => e.UserLikes)
                .WithMany(e => e.LikedPosts)
                .UsingEntity(
                    "UserLikesPost",
                    l => l.HasOne(typeof(Person)).WithMany().HasForeignKey("UsersId").HasPrincipalKey(nameof(Person.Id)),
                    r => r.HasOne(typeof(Post)).WithMany().HasForeignKey("PostsId").HasPrincipalKey(nameof(Post.Id)),
                    j => j.HasKey("PostsId", "UsersId"));

            modelBuilder.Entity<Post>()
                .HasOne(e => e.Creator)
                .WithMany(e => e.PublishedPosts)
                .HasForeignKey(e => e.CreatorId)
                .IsRequired();
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }

    }
}
