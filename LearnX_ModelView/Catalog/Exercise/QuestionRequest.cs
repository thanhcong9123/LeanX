using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Exercise
{
    public class QuestionRequest
    {
        public int QuestionId { get; set; }
        public string? QuestionText { get; set; }
        public List<AnswerRequest>? Answers { get; set; }
    }
}