using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Exercise
{
    public class ExerciseRequest
    {
        public int ExerciseId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề bài tập")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ0-9\s]+$", ErrorMessage = "Tiêu đề bài tập chỉ được chứa chữ cái, số và khoảng trắng, không bao gồm ký tự đặc biệt.")]
        public string? Title { get; set; }
        public int CourseId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn danh mục bài tập")]
        public string? Category { get; set; }
        public string? AnswerFile { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mô tả bài tập")]
        public string? Describe { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập hướng dẫn làm bài tập")]
        public string? Instruct { get; set; }
    }
}