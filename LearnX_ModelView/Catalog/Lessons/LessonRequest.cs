using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Lessons
{
    public class LessonRequest
    {
        public int LessonID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ID khoá học cho bài học")]
        public int CourseID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề bài học")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ0-9\s]+$", ErrorMessage = "Tiêu đề bài học chỉ được chứa chữ cái, số và khoảng trắng, không bao gồm ký tự đặc biệt.")]
        public string? LessonTitle { get; set; }
        public string? Content { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mục tiêu bài học")]
        public string? Objectives { get; set; }
        public string? VideoUrl { get; set; } 
        public DateTime StratDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ResourcesLessonRequest>? Resources { get; set; } // Danh sách tài nguyên liên quan đến bài học
        
    }
}