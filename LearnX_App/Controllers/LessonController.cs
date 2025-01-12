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
         [HttpPost]
        public async Task<IActionResult> AddLesson(Lesson newLesson)
        {

            await _lessonService.AddLessonAsync(newLesson);
            return RedirectToAction("Index", new { id = newLesson.CourseID });
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
                              Console.WriteLine("Id"+lessonID);

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
