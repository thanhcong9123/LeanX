using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LearnX_App.Models
{
    public class CreateEssayExerciseViewModel
    {
        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        [Display(Name = "Tiêu đề bài tập")]
        public string Title { get; set; } = string.Empty;

        [Required]
        public int CourseId { get; set; }

        [Display(Name = "Mô tả bài tập")]
        public string? Description { get; set; }

        [Display(Name = "File đáp án chuẩn")]
        public IFormFile? AnswerKeyFile { get; set; }

        [Display(Name = "Hướng dẫn làm bài")]
        public string? Instructions { get; set; }

        [Display(Name = "Thời gian làm bài (phút)")]
        public int? TimeLimit { get; set; }

        [Display(Name = "Điểm tối đa")]
        public decimal MaxScore { get; set; } = 10;
    }
}
