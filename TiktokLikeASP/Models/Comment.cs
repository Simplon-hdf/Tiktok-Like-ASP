using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TiktokLikeASP.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public bool IsVisible { get; set; }
        public Post Post { get; set; }
        public User User { get; set; }
    }
}
