using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Exercise
{
    public class ExerciseRequestWrapper
    {
        public ExerciseRequestWrapper() { }

        public ExerciseRequest ExerciseRequest { get; set; }
        public List<QuestionRequest> QuestionRequest { get; set; }
    }

}