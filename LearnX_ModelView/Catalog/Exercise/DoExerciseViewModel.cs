using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Exercise
{
    public class DoExerciseViewModel
    {
        public int ExerciseId { get; set; }
        public int CourseId { get; set; }
        public List<QuestionRequest> Questions { get; set; } = new List<QuestionRequest>();
        public Dictionary<int, int> UserAnswers { get; set; } = new Dictionary<int, int>();
        public bool Submitted { get; set; }
    }
}