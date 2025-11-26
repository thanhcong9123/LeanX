using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Exercise
{
    public class AnswerRequest
    {
        public int AnswerId { get; set; }
        [Required(ErrorMessage = "Nhập nội dung câu trả lời.")]
        public string? AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}