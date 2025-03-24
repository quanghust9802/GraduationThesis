using Microsoft.AspNetCore.Http;

namespace Application.Services.ImageServices
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
