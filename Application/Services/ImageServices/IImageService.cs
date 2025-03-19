using Application.Common.Paging;
using Application.DTOs.AuthDTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ImageServices
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
