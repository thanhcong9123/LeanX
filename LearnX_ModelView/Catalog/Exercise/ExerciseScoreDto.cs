using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Exercise
{
    public class ExerciseScoreDto
    {
        public int ExerciseId { get; set; }
        public string ExerciseTitle { get; set; }
        public decimal Score { get; set; }
        public bool IsPassed { get; set; }
        public DateTime DateCompleted { get; set; }
    }
}