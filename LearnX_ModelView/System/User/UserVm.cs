using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.System.User
{
    public class UserVm
    {
        public Guid Id { get; set; }

        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Tài khoản")]
        public string? UserName { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }
        public DateTime? DateJoined { get; set; }
        public DateTime? MemberDate { get; set; }
        public DateTime? Dob { get; set; }
        

        public IList<string>? Roles { get; set; }
    }
}