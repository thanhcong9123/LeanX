using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_ModelView.Common;
using LearnX_ModelView.System.User;
using Microsoft.AspNetCore.Identity.Data;

namespace LearnX_ApiIntegration.SystemService
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(Login loginRequest);
        Task<ApiResult<string>> Register(Register register);
        Task<ApiResult<string>> GetAll();
        Task<ApiResult<UserVm>> GetByID(Guid id);
        Task<ApiResult<bool>> DeleteAccount(Guid id);
        Task<ApiResult<bool>> UpdateUser(Guid id,UserUpdateRequest register);
    }
}