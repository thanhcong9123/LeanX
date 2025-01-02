using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Enrollment
{
    public class EnrollmentRequest
    {
        public Guid UserID { get; set; }  // Must be Guid to match AppUser's ID type
        public int CourseID { get; set; }  // Foreign key to Course
        public DateTime EnrollmentDate { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
}