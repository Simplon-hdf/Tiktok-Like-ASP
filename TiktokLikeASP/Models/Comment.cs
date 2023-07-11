using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace TiktokLikeASP.Models
{
    public class Comment
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("is_visible")]
        public bool IsVisible { get; set; }

        [Column("post")]
        public Post Post { get; set; }

        [Column("user")]
        public User User { get; set; }
    }
}
