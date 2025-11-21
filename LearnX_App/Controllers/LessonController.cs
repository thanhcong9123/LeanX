using System.Security.Claims;
using LearnX_ApiIntegration;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Lessons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Authorize]
    public class LessonController : Controller
    {
        private readonly ILessonApiClient _lessonService;

        public LessonController(ILessonApiClient lessonService)
        {
            _lessonService = lessonService;
        }
        // GET: LessonController
        public async Task<IActionResult> Index(int id)
        {
            var lessons = await _lessonService.GetAllLessonsAsync(id);
            var viewModel = new CourseDetailsViewModel
            {
                CourseId = id,
                Lessons = lessons
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create(int courseID)
        {
            var lesson = new LessonRequest();
            lesson.CourseID = courseID;
            return View(lesson);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LessonRequest request)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is not valid");
                return View(request);
            }


            try
            {
                // gọi service để lưu LessonRequest vào DB (thay bằng implementation của bạn)
                await _lessonService.AddLessonAsync(request); // ví dụ
                TempData["SuccessMessage"] = "Tạo bài học thành công.";
                return RedirectToAction("Details", "Course", new { id = request.CourseID });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Lỗi khi tạo bài học: " + ex.Message);
                return View(request);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteLesson(int lessonID)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid course data." });
                }
                Console.WriteLine("Id" + lessonID);

                // Logic để xử lý lưu course
                // Ví dụ: lưu vào database hoặc gọi một API
                bool isSaved = await _lessonService.DeleteLessonAsync(lessonID);

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

    }
}
