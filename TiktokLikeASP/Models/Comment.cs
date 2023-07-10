namespace TiktokLikeASP.Models
{
    public class Comment
    {
        public Guid id { get; set; }
        public string content { get; set; }
        public bool is_visible { get; set; }
        public Post post { get; set; }
        public User user { get; set; }
    }
}
