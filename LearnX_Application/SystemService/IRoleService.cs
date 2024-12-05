using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Common;

namespace LearnX_Application.SystemService
{
    public interface IRoleService
    {
         Task<List<AppRole>> GetAll();
        Task<ApiResult<bool>> CreateRole(AppRole appRole); 
    }
}