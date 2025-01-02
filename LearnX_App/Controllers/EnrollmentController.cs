using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LearnX_ApiIntegration;
using LearnX_ModelView.Catalog.Courses;
using LearnX_ModelView.Catalog.Enrollment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LearnX_App.Controllers
{
    [Route("[controller]")]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentClient _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EnrollmentController(IEnrollmentClient context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<IActionResult> Create(EnrollmentRequest enrollmentRequest)
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
                    enrollmentRequest.UserID = userIds;
                }
                Console.WriteLine("Id khoa hoc" + enrollmentRequest.CourseID);
                MyCourses myCourses = await _context.GetmyCourse(enrollmentRequest.UserID);
                var isAlreadySigned = myCourses.CourseSinged.Any(course => course.CourseID == enrollmentRequest.CourseID);
                var isMyCourse = myCourses.MyCourse.Any(course => course.CourseID == enrollmentRequest.CourseID);
                if (isAlreadySigned || isMyCourse)
                {
                    return Json(new { success = false, message = "You have already registered for this course." });
                }
                // Logic để xử lý lưu course
                // Ví dụ: lưu vào database hoặc gọi một API
                bool isSaved = await _context.Create(enrollmentRequest);

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
        [HttpGet("Outclass")]
        public async Task<IActionResult> Outclass(int idCourse)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid course data." });
                }
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Guid.TryParse(userId, out var userIds);

                OutclassRequest outclassRequest = new OutclassRequest()
                {
                    CourseID = idCourse,
                    UserID = userIds
                };
                Console.WriteLine("Id khoa hoc" + outclassRequest.CourseID);
                // Logic để xử lý lưu course
                // Ví dụ: lưu vào database hoặc gọi một API
                bool isSaved = await _context.Outclass(outclassRequest);
                if (isSaved != false)
                {
                    return Json(new { success = true, message = "Course Delete successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to out course." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }


    }
}