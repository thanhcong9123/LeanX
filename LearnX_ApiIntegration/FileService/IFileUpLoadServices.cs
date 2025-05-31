using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LearnX_ApiIntegration.FileService
{
    public interface IFileUpLoadServices
    {
        Task<string> UpLoadFile(IFormFile file);
    }
}