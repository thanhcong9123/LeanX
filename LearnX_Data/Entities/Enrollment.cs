using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentID { get; set; }
        public Guid UserID { get; set; }  // Must be Guid to match AppUser's ID type
        public AppUser? User { get; set; }  // Navigation property to AppUser
        public int CourseID { get; set; }  // Foreign key to Course
        public Course? Course { get; set; }  // Navigation property to Course
        public DateTime EnrollmentDate { get; set; }
        public double Progress { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}