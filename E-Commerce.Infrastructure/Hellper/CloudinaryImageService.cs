using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Application.Interfaces.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Hellper
{
    public class CloudinaryImageService : IImageService
    {
        private const int MaxImages = 5;
        private const long MaxSize = 2 * 1024 * 1024;
        private static readonly List<string> AllowedTypes = new()
        {
            "image/jpeg",
            "image/png"
        };
        private readonly Cloudinary _cloudinary;
        public CloudinaryImageService(IConfiguration configuration)
        {
            var account = new Account()
            {
                ApiKey = configuration["Cloudinary:ApiKey"],
                ApiSecret = configuration["Cloudinary:ApiSecret"],
                Cloud = configuration["Cloudinary:CloudinaryName"],
            };
            _cloudinary = new Cloudinary(account);
        }
        public async Task<List<string>> UploadImagesAsync(List<IFormFile> images)
        {
            var urls = new List<string>();
            foreach (var image in images)
            {
                using var stream = image.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(image.FileName, stream),
                    Folder = "E-CommerceApp/products"
                };
                var result = await _cloudinary.UploadAsync(uploadParams);
                urls.Add(result.SecureUrl.ToString());
            }
            return urls;
        }
        public BaseResponse Validate(List<IFormFile> images)
        {
            if (images == null || !images.Any())
                return new BaseResponse(false, 400,"At least one image is required");
            if (images.Count > MaxImages)
                return new BaseResponse(false, 400, $"Maximum {MaxImages} images allowed");
            foreach (var image in images)
            {
                if (!AllowedTypes.Contains(image.ContentType))
                    return new BaseResponse(false, 400, "Invalid image type");
                if (image.Length > MaxSize)
                    return new BaseResponse(false, 400, "Image size must be less than 2MB");
            }
            return new BaseResponse(true, 400, "Images are valid");
        }

        public async Task<BaseResponse> DeleteImageAsync(string imageUrl)
        {
            var publicId = GetPublicIdFromUrl(imageUrl);
            var deletionParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image
            };
            var result =await _cloudinary.DestroyAsync(deletionParams);
            if (result.Result != "ok") return new BaseResponse(false, 400, "Failed to delete image");
            return new BaseResponse(true, 200, "Image deleted successfully");
        }

        public string GetPublicIdFromUrl(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var fileName = Path.GetFileNameWithoutExtension(uri.AbsolutePath);
            var folder = "E-CommerceApp/products";
            return $"{folder}/{fileName}";
        }
    }
}
