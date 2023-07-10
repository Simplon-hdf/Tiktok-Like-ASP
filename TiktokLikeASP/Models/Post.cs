namespace TiktokLikeASP.Models
{
    public class Post
    {
        public Guid id { get; set; }
        public string title { get; set; }

        public string video_link { get; set; }

        public Boolean is_visible { get; set; }

        public List<Tag> Tags { get; } = new();

        public List<Users> Users { get; } = new();


        public List<Comments> Comments { get; } = new();
    }
}
