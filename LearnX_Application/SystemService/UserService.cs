using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LearnX_Application.Base;
using LearnX_Data.EF;
using LearnX_Data.Entities;
using LearnX_ModelView.Common;
using LearnX_ModelView.System.User;
using LearnX_Utilities.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LearnX_Application.SystemService
{
    public class UserService : EntityBaseRepository<AppUser>, IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userInManager;
        private readonly IConfiguration _config;
        public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager,
                            SignInManager<AppUser> signInManager
                            , IConfiguration config
                            , UserManager<AppUser> userInManager, LearnXDbContext context) : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;
            _userInManager = userInManager;
        }

        public async Task AppRoleFOrUser(string userName, string id)
        {
            var users = await _userInManager.FindByIdAsync(userName);
            if (users == null)
            {
                throw new MyClassException($"Khong tim thaays");
            }
            var roles = await _userInManager.GetRolesAsync(users);
            var role = await _roleManager.FindByIdAsync(id);
            await _userInManager.AddToRoleAsync(users, role.Name);
            foreach (var rolename in roles)
            {
                if (role.Name.Contains(rolename)) continue;
                await _userInManager.RemoveFromRoleAsync(users, rolename);
            }
        }

        public async Task<ApiResult<string>> Authencate(Login request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName!);
            if (user == null) return new ApiErrorResult<string>("Tài khoản không tồn tại");
            var result = await _signInManager.PasswordSignInAsync(user, request.Password!, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return new ApiErrorResult<string>("Đăng nhập không đúng");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {

                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role, string.Join(";",roles)),
                new Claim(ClaimTypes.Name, request.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // user.Id là Guid
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
            return new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));

        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<bool>("User không tồn tại");
            }
            var reult = await _userManager.DeleteAsync(user);
            if (reult.Succeeded)
                return new ApiSuccessResult<bool>();

            return new ApiErrorResult<bool>("Xóa không thành công");
        }

        public async Task<List<AppUser>> GetAll()
        {
            var listUser = await _userManager.Users.ToListAsync();
            if (listUser == null) throw new MyClassException($"cannot find any user!");
            return listUser;
        }

        public async Task<ApiResult<UserVm>> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return new ApiErrorResult<UserVm>("User không tồn tại");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = new UserVm()
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                Roles = roles,
                MemberDate = user.Member,
                DateJoined = user.DateJoined,
                

            };
            return new ApiSuccessResult<UserVm>(userVm);
        }

        public async Task<ApiResult<bool>> Register(Register request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                return new ApiErrorResult<bool>("Tài khoản đã tồn tại");
            }
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return new ApiErrorResult<bool>("Emai đã tồn tại");
            }
            if (request.Password != request.ConfirmPassword)
            {
                return new ApiErrorResult<bool>("Mật khẩu xác nhận không đúng");
            }
            user = new AppUser()
            {
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = true,
                DateJoined = DateTime.Now,
                Member = DateTime.Now,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Dob = request.Dob                
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            var roles = await _userInManager.GetRolesAsync(user);
            var role = await _roleManager.FindByIdAsync("CECA3754-C5FB-4AB4-FA05-08DE1C8CB0D5");
            await _userInManager.AddToRoleAsync(user, role.Name);
            foreach (var rolename in roles)
            {
                await _userInManager.RemoveFromRoleAsync(user, rolename);
            }
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Đăng ký không thành công");
        }

        public async Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != id))
            {
                return new ApiErrorResult<bool>("Emai đã tồn tại");
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<bool>();
            }
            return new ApiErrorResult<bool>("Cập nhật không thành công");
        }
    }
}