namespace TiktokLikeASP.DistantData
{
    public static class CreateFile
    {
        public static FileStream NewFile(this string filename, string extension)
        {
            return File.Create($"/TempVideo{filename}{extension}");
        }
    }
}
