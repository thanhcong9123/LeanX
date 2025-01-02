using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Lessons
{
    public class LessonRequest
    {
         public int LessonID { get; set; }
        public int CourseID { get; set; }
        public string? LessonTitle { get; set; }
        public string? Content { get; set; }
    }
}