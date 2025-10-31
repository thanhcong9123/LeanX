using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.System.User
{
    public class Register
    {
        [Required]
        [Display(Name = "Tên")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]+$", ErrorMessage = "Tên chỉ được chứa chữ cái và khoảng trắng, không bao gồm số hoặc ký tự đặc biệt.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Họ")]
        [RegularExpression(@"^[a-zA-ZÀ-ỹ\s]+$", ErrorMessage = "Họ chỉ được chứa chữ cái và khoảng trắng, không bao gồm số hoặc ký tự đặc biệt.")]
        public string LastName { get; set; }

        [Display(Name = "Ngày sinh")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [Display(Name = "Hòm thư")]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [Required]
        public string PhoneNumber { get; set; }

        [Display(Name = "Tài khoản")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{6,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự, 1 chữ hoa, 1 số và 1 ký tự đặc biệt.")]
        public string Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}