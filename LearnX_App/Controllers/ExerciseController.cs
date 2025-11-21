using System.Security.Claims;
using LearnX_ApiIntegration;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Exercise;
using LearnX_ModelView.Catalog.Scores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{

    public class ExerciseController : Controller
    {
        private readonly IExerciseApiClient _exerciseService;
        private readonly ICourseApiClient _courseService;
        private readonly IScoreApiClient _scoreService;
        private readonly IEssaySubmissionApiClient _essaySubmissionService;

        [ActivatorUtilitiesConstructor]
        public ExerciseController(
            IExerciseApiClient exerciseService,
            ICourseApiClient courseApiClient,
            IScoreApiClient scoreService,
            IEssaySubmissionApiClient essaySubmissionService)
        {
            _exerciseService = exerciseService;
            _courseService = courseApiClient;
            _scoreService = scoreService;
            _essaySubmissionService = essaySubmissionService;
        }
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create(int CourseId)
        {
            var model = new ExerciseRequestWrapper
            {
                ExerciseRequest = new ExerciseRequest
                {
                    CourseId = CourseId
                }
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExerciseRequestWrapper model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                // gọi service để lưu ExerciseRequest vào DB (thay bằng implementation của bạn)
                var result = await _exerciseService.AddExerciseAsync(model);
                TempData["SuccessMessage"] = "Tạo bài tập thành công.";
                return RedirectToAction("Details", "Exercise", new { id = model.ExerciseRequest.CourseId });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Lỗi khi tạo bài tập: " + ex.Message);
                return View(model);
            }
        }

        // GET: ExerciseController
        public async Task<ActionResult> Index(int id)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid.TryParse(userId, out var userIds);
            if (userId != null)
            {
                var lessons = await _exerciseService.GetAll(userIds);
                if (lessons != null)
                {
                    return View(lessons);
                }
            }

            return View();
        }

        public async Task<ActionResult> Details(int id)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Course");
            }
            var courseofUser = await _courseService.GetmyCourse(Guid.Parse(userId));
            if (courseofUser == null)
            {
                return RedirectToAction("Index", "Course");

            }
            var isInMyCourse = courseofUser.MyCourse.Any(c => c.CourseID == id);
            var isInCourseSigned = courseofUser.CourseSinged.Any(c => c.CourseID == id);
            if (!isInMyCourse && !isInCourseSigned)
            {
                // Nếu không thuộc cả hai danh sách, thông báo lỗi hoặc chuyển hướng
                TempData["Error"] = "Bạn không có quyền truy cập vào khóa học này.";
                return RedirectToAction("Index", "Course");
            }
            ViewBag.isInMyCourse = isInMyCourse;
            ViewBag.isInCourseSigned = isInCourseSigned;
            var courseDetails = await _exerciseService.GetExerciseDetailsAsync(id);
            if (courseDetails == null)
            {
                TempData["Error"] = "Không tìm thấy khóa học.";
                return View();
            }
            ViewBag.CourseId = id;
            return View(courseDetails);
        }
        [Authorize]
        public async Task<ActionResult> DoExercise(int exerciseId)
        {

            // Kiểm tra loại bài tập
            var isEssayExercise = await _exerciseService.IsEssayExercise(exerciseId);

            if (isEssayExercise)
            {
                // Chuyển hướng đến action làm bài tự luận
                return RedirectToAction("DoEssayExercise", "EssaySubmission", new { exerciseId });
            }

            // Lấy danh sách câu hỏi của bài tập trắc nghiệm
            var questions = await _exerciseService.getQuestion(exerciseId);
            if (questions == null)
            {
                TempData["Error"] = "Không có câu hỏi trong bài tập này.";
                return RedirectToAction("Index", "Course");
            }

            var viewModel = new DoExerciseViewModel
            {
                ExerciseId = exerciseId,
                Questions = questions.Select(q => new QuestionRequest
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    Answers = q.Answers?.Select(a => new AnswerRequest
                    {
                        AnswerId = a.AnswerId,
                        AnswerText = a.AnswerText,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList()
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> SubmitAnswers(DoExerciseViewModel model)
        {
            if (model == null || model.UserAnswers == null)
            {
                ViewBag.Error = "Dữ liệu không hợp lệ.";
                return RedirectToAction("Index", "Course");
            }
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid.TryParse(userId, out var userIds);
            if (string.IsNullOrEmpty(userId))
            {
                ViewBag.Error = "Bạn cần đăng nhập để nộp bài tập.";
                return RedirectToAction("Index", "Course");
            }
            if (model.Questions == null || !model.Questions.Any())
            {
                var questions = await _exerciseService.getQuestion(model.ExerciseId);
                model.Questions = questions.Select(q => new QuestionRequest
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    Answers = q.Answers?.Select(a => new AnswerRequest
                    {
                        AnswerId = a.AnswerId,
                        AnswerText = a.AnswerText,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList();
            }
            var modelSubmit = new SubmitExerciseRequest()
            {
                ExerciseId = model.ExerciseId,
                UserId = userIds,
                Questions = model.Questions.Select(ua => new QuestionSumit
                {
                    QuestionId = ua.QuestionId,
                    SelectedAnswerId = model.UserAnswers[ua.QuestionId]
                }).ToList()

            };
            foreach (var item in modelSubmit.Questions)
            {
                Console.WriteLine("Keets quar " + item.SelectedAnswerId);
            }
            // Gửi dữ liệu nộp bài tập đến service để xử lý
            var result = await _exerciseService.SubmitExerciseAsync(modelSubmit);
            var score = result;
            var isPassed = score >= 5;
            // Cập nhật điểm số và trạng thái
            ViewBag.Score = score;
            ViewBag.IsPassed = isPassed;
            var scoreRequest = new ScoreRequest
            {
                IdUser = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                ExerciseId = model.ExerciseId,
                DateCompleted = DateTime.UtcNow,  // Ngày hoàn thành bài tập
                Score = score,
                IsPassed = isPassed
            };
            // Lưu điểm số qua API
            bool isScoreSaved = await _scoreService.AddScoreAsync(scoreRequest);
            if (!isScoreSaved)
            {

                ViewBag.Error = "Lưu điểm số thất bại.";
                return RedirectToAction("Index", "Exercise");
            }
            // Trả kết quả về View
            model.Submitted = true;
            return View("Results", model);
        }



        [HttpGet("Delete/{id}/{idCourse}")]
        public async Task<IActionResult> Delete(int id, int idCourse)
        {
            var result = await _exerciseService.DeleteExerciseAsync(id);
            if (result)
            {
                return RedirectToAction("Details", "Exercise", new { id = idCourse });
            }
            else
            {
                return RedirectToAction("Details", "Exercise", new { id = idCourse });
            }
        }

    }
}
