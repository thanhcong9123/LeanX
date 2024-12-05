using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }
        public string? AnswerText { get; set; }
        public bool IsCorrect { get; set; } // Đánh dấu đáp án đúng

        // Một đáp án thuộc về một câu hỏi
        public int QuestionId { get; set; }
        public Question? Question { get; set; }
    }
}