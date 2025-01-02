using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Enrollment
{
    public class OutclassRequest
    {
        public Guid UserID { get; set; }  // Must be Guid to match AppUser's ID type
        public int CourseID { get; set; } 
    }
}