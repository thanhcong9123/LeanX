using LearnX_ApiIntegration;
using LearnX_ApiIntegration.SystemService;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
        options.AccessDeniedPath = "/User/Forbidden/";
    });
    
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session tồn tại

});

builder.Services.AddHttpContextAccessor(); // Đăng ký HttpContextAccessor

builder.Services.AddTransient<IUserApiClient, UserApiClient>();
builder.Services.AddTransient<ICourseApiClient, CourseApiClient>();
builder.Services.AddTransient<IEnrollmentClient, EnrollmentClient>();
builder.Services.AddTransient<IExerciseApiClient, ExerciseApiClient>();
builder.Services.AddTransient<ILessonApiClient, LessonApiClient>();
builder.Services.AddTransient<IScoreApiClient, ScoreApiClient>();
builder.Services.AddTransient<IMessageApiClient, MessageApiClient>();
builder.Services.AddRazorPages();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseCookiePolicy();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Course}/{action=Home}/{id?}");
app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=User}/{action=Login}/{id?}");
app.Run();
