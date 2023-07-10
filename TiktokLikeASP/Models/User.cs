using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace TiktokLikeASP.Models
{
    public class User
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("is_visible")]
        public bool IsVisible { get; set; }

        /*               RELATIONS                  */

        [Column("published_posts")]
        public List<Post> PublishedPosts { get; set; }
        
        [Column("written_comments")]
        public List<Comment> WrittenComments { get; set; }

        [Column("liked_posts")]
        public List<Post> LikedPosts { get; set; }

        [Column("followers")]
        public List<Follow> Followers { get; set; } //Peoples that follow us
        
        [Column("users_followed")]
        public List<Follow> UsersFollowed { get; set; } // People that we follow

    }
}
