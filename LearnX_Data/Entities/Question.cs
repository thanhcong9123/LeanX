using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public string? QuestionText { get; set; }

        // Một câu hỏi thuộc về một bài tập
        public int ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }

        // Một câu hỏi có nhiều đáp án
        public ICollection<Answer>? Answers { get; set; }
    }
}