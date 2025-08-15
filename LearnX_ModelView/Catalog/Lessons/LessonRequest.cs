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
        public string? Objectives { get; set; }
        public string? VideoUrl { get; set; } 
        public DateTime StratDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ResourcesLessonRequest>? Resources { get; set; } // Danh sách tài nguyên liên quan đến bài học
        
    }
}