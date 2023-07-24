namespace TiktokLikeASP.Models.ViewModels
{
    public class EditProfileRequest
    {
        public string Username { get; set; } 
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
