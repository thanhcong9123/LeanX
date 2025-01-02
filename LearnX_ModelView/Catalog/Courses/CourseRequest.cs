using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Courses
{
    public class CourseRequest
    {
        public string? CourseName { get; set; }
        public string? Description { get; set; }
        public Guid InstructorID { get; set; }  // Foreign key to AppUser (Instructor)
        public int CategoryID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
    }
}