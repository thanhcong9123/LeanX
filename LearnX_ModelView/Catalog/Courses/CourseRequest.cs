using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Courses
{
    public class CourseRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập tên khóa học")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]+$", ErrorMessage = "Tên khoá học chỉ được chứa chữ cái và khoảng trắng, không bao gồm số hoặc ký tự đặc biệt.")]
        public string? CourseName { get; set; }
        [Required(ErrorMessage = "Vui lòng mô tả khoá học")]
        public string? Description { get; set; }
        [Required]
        public Guid InstructorID { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn danh mục cho khoá học")]
        public int CategoryID { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "Ngày kết thúc")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public string? Prerequisites { get; set; }

    }
}