using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LearnX_Data.Entities
{
    public class AppRole: IdentityRole<Guid>
    {
         public AppRole(){}
        public AppRole(string appRole)
        {
            this.Name = appRole;
        }
    }
}