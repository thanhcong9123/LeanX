using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Exercise
{
    public class AIGeneratedQuestionsResponse
    {
        public List<GeneratedQuestion> Questions { get; set; }

    }
     public class GeneratedQuestion
    {
        public string QuestionText { get; set; }
        public List<GeneratedAnswer> Answers { get; set; }
    }

    public class GeneratedAnswer
    {
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}