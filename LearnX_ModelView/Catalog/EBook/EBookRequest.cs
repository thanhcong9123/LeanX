using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LearnX_ModelView.Catalog.EBook
{
    public class EBookRequest
    {
        public string IFormFile { get; set; }
        public string Title { get; set; }
        public string Description { get; set; } 
        public string ImgPath { get; set; }
    }
}