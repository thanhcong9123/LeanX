using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LearnX_Data.EF;
using LearnX_Data.Entities;
using LearnX_ApiIntegration;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using LearnX_ModelView.Catalog.Courses;

namespace LearnX_App.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ICourseApiClient _context;
        private readonly IScoreApiClient _scoreApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IExerciseApiClient _exerciseApiClient;
        private readonly IEssaySubmissionApiClient _essaySubmissionApiClient;

        public CourseController(ICourseApiClient context, IHttpContextAccessor httpContextAccessor,
         IScoreApiClient scoreApiClient, IExerciseApiClient exerciseApiClient, IEssaySubmissionApiClient essaySubmissionApiClient)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _scoreApiClient = scoreApiClient;
            _exerciseApiClient = exerciseApiClient;
            this._essaySubmissionApiClient = essaySubmissionApiClient;
        }
        public IActionResult Home()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> UserofCourse(int id)
        {
            var mode = await _context.GetUserCourse(id);
            return View(mode);
        }
        // GET: Course
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid.TryParse(userId, out var userIds);
            var courses = await _context.GetmyCourse(userIds);
            if (courses == null)
            {
                return View();
            }
            var exercises = await _exerciseApiClient.GetAll(userIds);
            var essaySubmissions = await _essaySubmissionApiClient.GetEssaySubmissionsByUserAsync(userIds);
            var scoers = await _scoreApiClient.GetScoreAsync(userIds);
            var data = new LearnX_ModelView.Catalog.ViewApp.HomeModel
            {
                Courses = courses.CourseSinged,
                Exercises = exercises,
                ExercisesNotDone = exercises.Where(e => !scoers.Any(s => s.ExerciseId == e.ExerciseId)
                && !essaySubmissions.Any(es => es.ExerciseId == e.ExerciseId)).ToList()
            };
            var userName = User.Identity?.Name; // Lấy FullName từ Claims

            ViewData["ActivePage"] = userName;
            return View(data);


        }
        [Authorize]
        public async Task<IActionResult> Exercise()
        {
            ViewData["ActivePage"] = "Exercise";

            return View();
        }
        [Authorize]
        // GET: Course/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if ( id == 0)
            {
                return NotFound();
            }
            var course = await _context.GetbyId(id);
            return View(course);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_CreateCourseForm");
        }
        [HttpGet]
        public async Task<IActionResult> CreateCourse(CourseRequest course)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid course data." });
                }
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Guid.TryParse(userId, out var userIds);
                if (userId != null)
                {
                    course.InstructorID = userIds;
                }
                // Logic để xử lý lưu course
                // Ví dụ: lưu vào database hoặc gọi một API
                bool isSaved = await _context.Create(course);
                // Console.WriteLine(course.CourseName+course.CategoryID+course.Description+course.InstructorID);
                if (isSaved != false)
                {
                    return Json(new { success = true, message = "Course created successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to create course." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }

        }

        // GET: Course/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {

            return View();
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,CourseName,Description,InstructorID,CategoryID,StartDate,EndDate,Price")] Course course)
        {

            return View(course);
        }

        // GET: Course/Delete/5


        // POST: Course/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int courseId)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid course data." });
                }

                // Logic để xử lý lưu course
                // Ví dụ: lưu vào database hoặc gọi một API
                bool isSaved = await _context.Delete(courseId);

                Console.WriteLine("id" + courseId);
                if (isSaved != false)
                {
                    return Json(new { success = true, message = "Course Delete successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to delete course." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }

        private Guid GetUserIdFromToken()
        {
            // Lấy token từ session
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");

            // Giải mã token và lấy claim 'sub' hoặc 'nameid' là ID người dùng
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;
                var userId = jsonToken?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                return userId != null ? Guid.Parse(userId) : Guid.Empty;
            }
            return Guid.Empty;
        }


    }
}
