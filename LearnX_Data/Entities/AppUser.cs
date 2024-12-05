using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LearnX_Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public DateTime DateJoined { get; set; }
        public DateTime LastLogin { get; set; }
        public ICollection<Enrollment>? Enrollments { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<Course>? Courses { get; set; }


    }
}