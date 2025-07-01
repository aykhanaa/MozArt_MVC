

namespace Final_MozArt.Helpers.Extensions
{
    public static class FileExtension
    {
        public static bool CheckFileType(this IFormFile file, string pattern)
        {
            return file.ContentType.StartsWith(pattern);
        }

        public static bool CheckFileSize(this IFormFile file, int maxKb)
        {
            return file.Length / 1024 <= maxKb;
        }

        public static async Task SaveFileAsync(this IFormFile file, string path)
        {
            using FileStream stream = new(path, FileMode.Create);
            await file.CopyToAsync(stream);
        }

        public static string GetFilePath(this IWebHostEnvironment env, string folder, string fileName)
        {
            return Path.Combine(env.WebRootPath, folder, fileName);
        }
    }
}
