﻿namespace ECommerce.Helpers
{
    public class UploadFileHelper
    {
        public async static Task<string> UploadFile(IFormFile file)
        {
            var imagePath = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string path = @$"wwwroot\{imagePath}";

            using (var fs = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite))
            {
                await file.CopyToAsync(fs);
            }

            return imagePath;
        }
    }
}
