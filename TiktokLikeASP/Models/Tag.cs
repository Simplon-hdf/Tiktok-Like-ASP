namespace TiktokLikeASP.Models
{
    public class Tag
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public List<Post> posts { get; set; }
    }
}
