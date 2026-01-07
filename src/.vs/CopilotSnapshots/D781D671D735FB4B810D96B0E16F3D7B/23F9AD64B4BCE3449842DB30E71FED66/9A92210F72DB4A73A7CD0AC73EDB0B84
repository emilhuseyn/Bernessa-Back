using App.Business.Services.ExternalServices.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Business.Services.ExternalServices.Abstractions
{
    public class FileManagerService : IFileManagerService
    {
        private readonly IWebHostEnvironment _environment;

        public FileManagerService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public bool BeAValidImage(IFormFile file)
        {
            return file != null && file.ContentType.Contains("image") && 1024 * 1024 * 5 >= file.Length;
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            string fileName = await UploadLocalAsync(file);

            return "/images/" + fileName;
        }

        public async Task<string> UploadLocalAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("The file parameter cannot be null.");

            if (!BeAValidImage(file))
                throw new Exception("The file format is not valid. You have to upload image type file and it should be maximum 5MB.");

            var fileName = Guid.NewGuid().ToString() + "_" +
                Path.GetFileNameWithoutExtension(file.FileName) +
                Path.GetExtension(file.FileName);

            var imagesPath = Path.Combine(_environment.WebRootPath, "images");

            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            var filePath = Path.Combine(imagesPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}
