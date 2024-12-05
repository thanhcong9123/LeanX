using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Application.Base;
using LearnX_Data.Entities;
using LearnX_ModelView.Common;
using LearnX_ModelView.System.User;

namespace LearnX_Application.SystemService
{
    public interface IUserService : IEntityBaseRepository<AppUser>
    {
        Task<ApiResult<string>> Authencate(Login request);

        Task<ApiResult<bool>> Register(Register request);

        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);

        Task<ApiResult<UserVm>> GetById(Guid id);

        Task<ApiResult<bool>> Delete(Guid id);
        Task<List<AppUser>> GetAll();
        Task AppRoleFOrUser(string userName, string id);
    }
}