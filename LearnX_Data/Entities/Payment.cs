using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }
        public Guid UserID { get; set; }
        public AppUser? User { get; set; }
        public int CourseID { get; set; }
        public Course? Course { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }

    }
}