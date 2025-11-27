using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LearnX_ApiIntegration.SystemService;
using LearnX_ModelView.System.User;
using LearnX_Utilities.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace LearnX_App.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiClient _context;
        private readonly IConfiguration _configuration;


        public UserController(IUserApiClient context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [Authorize]
        public async Task<IActionResult> InfoUser()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid.TryParse(userId, out var id);
            var user = await _context.GetByID(id);
            if (user.IsSuccessed)
            {
                var model = new UserVm()
                {
                    Id = user.ResultObj.Id,
                    UserName = user.ResultObj.UserName,
                    Email = user.ResultObj.Email,
                    PhoneNumber = user.ResultObj.PhoneNumber,
                    DateJoined = user.ResultObj.DateJoined,
                    MemberDate = user.ResultObj.MemberDate,
                    Roles = user.ResultObj.Roles,
                    Dob = user.ResultObj.Dob
                };

                return View(model);
            }
            return RedirectToAction("Login", "User");

        }
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        // GET: User
        [HttpPost]
        public async Task<IActionResult> Login(LearnX_ModelView.System.User.Login request)
        {

            if (!ModelState.IsValid)
                return View(request);


            var result = await _context.Authenticate(request);
            if (result.ResultObj == null)
            {
                ModelState.AddModelError("", "Tên đăng nhập sai hoặc không đúng mật khẩu");
                return View();
            }
            var userPrincipal = this.ValidateToken(result.ResultObj);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(100),
                IsPersistent = false
            };
            // Lưu token vào Session
            // HttpContext.Session.SetString("Token", result.ResultObj);

            // Lưu các thông tin cần thiết vào session
            HttpContext.Session.SetString(SystemConstants.AppSettings.Token, result.ResultObj);
            // Thực hiện đăng nhập

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);

            return RedirectToAction("Index", "Course");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(LearnX_ModelView.System.User.Register register)
        {
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }
                }
                return View(register);
            }
            var result = await _context.Register(register);
            if (result.IsSuccessed)
            {
                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập để tiếp tục.";

                return RedirectToAction("Login");

            }

            ModelState.AddModelError("", result.Message);

            return View(register);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login", "User");
        }
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }
        private Guid GetUserIdFromPrincipal(ClaimsPrincipal principal)
        {
            // Lấy claim 'sub' hoặc 'nameid' (tùy theo API của bạn cấu hình)
            var userIdClaim = principal?.FindFirst(ClaimTypes.NameIdentifier) ?? principal?.FindFirst("sub");

            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }

            return Guid.Empty;
        }
    }
}
