using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class Category
    {
        [Key]

        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }

        public ICollection<Course>? Courses { get; set; }
    }
}