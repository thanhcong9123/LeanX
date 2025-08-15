using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class Lesson
    {
        [Key]
        public int LessonID { get; set; }
        public int CourseID { get; set; }
        public Course? Course { get; set; }
        public string? LessonTitle { get; set; }
        public string? Content { get; set; }
        public string? Objectives { get; set; }
        public string? VideoUrl { get; set; } 
        public DateTime StratDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<ResourcesLesson>? Resources { get; set; }  // Navigation property to ResourcesLesson
    }
}