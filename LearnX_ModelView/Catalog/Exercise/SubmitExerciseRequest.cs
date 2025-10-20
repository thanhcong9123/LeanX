using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Exercise
{
    public class SubmitExerciseRequest
    {
        public int ExerciseId { get; set; }
        public Guid UserId { get; set; }
        public List<QuestionSumit> Questions { get; set; }
    }
}