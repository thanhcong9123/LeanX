using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.System.User
{
    public class Login
    {
         public string? UserName { get; set; }

        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
}