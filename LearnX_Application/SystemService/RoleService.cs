using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using LearnX_Data.Entities;
using LearnX_ModelView.Common;
using LearnX_Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LearnX_Application.SystemService
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<ApiResult<bool>> CreateRole(AppRole appRole)
        {
            if (!_roleManager.RoleExistsAsync(appRole.Name).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new AppRole(appRole.Name)).GetAwaiter().GetResult();
            }
            return new ApiErrorResult<bool>("thành công");

        }

        public async Task<List<AppRole>> GetAll()
        {
            var listRole = await _roleManager.Roles.ToListAsync();
            if (listRole == null) throw new MyClassException($"cannot find any Role!");
            return listRole;
        }
    }
}