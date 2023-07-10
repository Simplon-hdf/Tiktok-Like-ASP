namespace TiktokLikeASP.Models
{
    public class Post
    {
        public Guid id { get; set; }
        public string title { get; set; }

        public string video_link { get; set; }

        public bool is_visible { get; set; }

        public List<Tag> Tags { get; } = new();

        public User Users { get; } = new();


        public List<Comment> Comments { get; } = new();
    }
}
