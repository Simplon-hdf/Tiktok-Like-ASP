using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiktokLikeASP.Context;
using TiktokLikeASP.Models;

namespace TiktokLikeASP.ViewComponents
{
    public class PostViewComponent : ViewComponent
    {
        private ApplicationDbContext _context;

        public PostViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid postId)
        {
            ViewBag.CurrentPost = await GetPost(postId);
            return View("SelectedPost");
        }

        private async Task<Post> GetPost(Guid postId)
        {
            try
            {
                var post = await _context.Posts.Include(p => p.Creator).Where(p => p.Id == postId).FirstAsync();
                return post;
            } catch (Exception)
            {
                throw new Exception("PostViewComponent: Couldn't find post from given ID");
            }
        }
    }
}
