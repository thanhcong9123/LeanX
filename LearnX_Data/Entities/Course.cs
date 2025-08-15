using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }
        public string? CourseName { get; set; }
        public string? Description { get; set; }
        public Guid InstructorID { get; set; }  // Foreign key to AppUser (Instructor)
        public AppUser? Instructor { get; set; }  // Navigation property to AppUser

        public int CategoryID { get; set; }
        public Category? Category { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public string? Prerequisites { get; set; } 
        


        // Navigation properties
        public ICollection<Lesson>? Lessons { get; set; }
        public ICollection<Enrollment>? Enrollments { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Exercise>? Exercises { get; set; }
    }
}