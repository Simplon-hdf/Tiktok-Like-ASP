namespace TiktokLikeASP.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsVisible { get; set; }
    }
}
