using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_App.Models
{
    public class EbookViewModel
    {
        [Required(ErrorMessage = "Nhập tên sách.")]
        public string? Title { get; set; }
        
        public string? imgPath { get; set; }
        public string? LinkYoutube { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Please upload a PDF file.")]
        public IFormFile? FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
        public int CountPages { get; set; }
        public string? Author { get; set; }
        public string? Status { get; set; } 
        public string? NameCategory { get; set; }
    }
}