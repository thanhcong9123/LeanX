using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace LearnX_ApiIntegration.FileService
{
    public class LoaclFileUpLoadService :IFileUpLoadServices
    {
         private readonly IHostEnvironment _environment;
        public LoaclFileUpLoadService(IHostEnvironment environment)
        {
            this._environment = environment;
        }
        public async Task<string> UpLoadFile(IFormFile file)
        {
            var filepath = Path.Combine(_environment.ContentRootPath,@"wwwroot/FileBook", file.FileName);
            using var fileStream = new FileStream(filepath, FileMode.Create);
            await  file.CopyToAsync(fileStream);
            return file.FileName;
        }
    }
}