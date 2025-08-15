using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LearnX_ModelView.Catalog.EBook
{
    public class EBookRequest
    {
         public string Title { get; set; }
        public string imgPath { get; set; }
        public string LinkYoutube { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
        public int CountPages { get; set; }
        public string Author { get; set; }
        public string Status { get; set; } 
        public string? NameCategory { get; set; }
    }
}