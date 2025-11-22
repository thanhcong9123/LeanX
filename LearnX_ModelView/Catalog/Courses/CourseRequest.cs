using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Courses
{
    public class CourseRequest
    {
        [Required]
        public string? CourseName { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public Guid InstructorID { get; set; }

        public int CategoryID { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "Ngày kết thúc")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public string? Prerequisites { get; set; }

    }
}