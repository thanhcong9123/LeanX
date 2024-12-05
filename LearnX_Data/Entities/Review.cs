using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }
        public Guid UserID { get; set; }
        public AppUser? User { get; set; }
        public int CourseID { get; set; }
        public Course? Course { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime DatePosted { get; set; }
    }
}