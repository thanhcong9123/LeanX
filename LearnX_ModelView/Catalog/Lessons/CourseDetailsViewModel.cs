using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;

namespace LearnX_ModelView.Catalog.Lessons
{
    public class CourseDetailsViewModel
    {
        public int CourseId { get; set; }
        public List<Lesson> Lessons { get; set; }
    }
}