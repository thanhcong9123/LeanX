using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.System.User
{
    public class UserUpdateRequest
    {
         public Guid Id { get; set; }

        [Display(Name = "Hòm thư")]
        public string? Email { get; set; }

        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }
    }
}