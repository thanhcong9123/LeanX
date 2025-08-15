using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Lessons
{
    public class ResourcesLessonRequest
    {
        public string? ResourceName { get; set; }
        public string? ResourceType { get; set; }  // e.g., Video, Document, Link
        public string? ResourceUrl { get; set; }  // URL or path to the resource
    }
}