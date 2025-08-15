using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class EBook
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string imgPath { get; set; }
        public string LinkYoutube { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
        public int CountPages { get; set; }
        public string Author { get; set; }
        public int View { get; set; } 
        public string Status { get; set; } = "Free"; // Default status is Active
        public string? NameCategory { get; set; }
        public ICollection<EvaluateBook>? EvaluateBooks { get; set; }


        public EBook()
        {
        }
    }
}