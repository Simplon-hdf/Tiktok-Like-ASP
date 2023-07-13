using Microsoft.AspNetCore.Mvc;
using TiktokLikeASP.Context;
using TiktokLikeASP.Models.ViewModels;
using TiktokLikeASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using RestSharp;
using System.Web;

namespace TiktokLikeASP.DistantData
{
    public static class Uploader
    {
        public static string UploadVideo(IFormFile videoFile)
        {
            if (videoFile != null && videoFile.Length > 0)
            {
                // Generate a unique file name or use the original file name
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(videoFile.FileName);

                // Specify the path where you want to save the video file
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Videos", fileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    videoFile.CopyTo(stream);
                }

                // Optionally, you can perform additional processing or save information about the uploaded video to a database

                return fileName;
            }

            return "";
        }
    }
}
