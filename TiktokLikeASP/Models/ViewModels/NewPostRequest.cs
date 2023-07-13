using Microsoft.AspNetCore.Mvc;

namespace TiktokLikeASP.Models.ViewModels
{
    public class NewPostRequest
    {
        public string Title { get; set; }
        public IFormFile VideoLink { get; set; }

    }
}
