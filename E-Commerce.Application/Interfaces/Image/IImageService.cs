using E_Commerce.Application.DTOs.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.Image
{
    public interface IImageService
    {
        Task<List<string>> UploadImagesAsync(List<IFormFile> images);
        BaseResponse Validate(List<IFormFile> images);
        Task<BaseResponse> DeleteImageAsync(string imageUrl);
        string GetPublicIdFromUrl(string imageUrl);

    }
}
