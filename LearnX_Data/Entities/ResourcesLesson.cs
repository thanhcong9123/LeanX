using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class ResourcesLesson
    {
        public int Id { get; set; }
        public int LessonID { get; set; }  // Foreign key to Lesson
        public Lesson? Lesson { get; set; }  // Navigation property to Lesson

        public string? ResourceName { get; set; }
        public string? ResourceType { get; set; }  // e.g., Video, Document, Link
        public string? ResourceUrl { get; set; }  // URL or path to the resource
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}