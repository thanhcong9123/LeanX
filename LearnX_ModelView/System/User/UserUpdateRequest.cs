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
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public DateTime Dob { get; set; }
        public DateTime Member { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime? PremiumUntil { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }


    }
}