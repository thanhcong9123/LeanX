using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Exercise
{
    public class ExerciseRequest
    {
        public int ExerciseId { get; set; }
        public string? Title { get; set; }
        public int CourseId { get; set; }
        public string? Category { get; set; }
        public string? AnswerFile { get; set; }
        public string? Describe { get; set; }
        public string? Instruct { get; set; }
    }
}