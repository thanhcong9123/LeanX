using System.Security.Claims;
using LearnX_ApiIntegration;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Exercise;
using LearnX_ModelView.Catalog.Scores;
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
        public IActionResult CreateExercise(int CourseId)
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
        public async Task<ActionResult> DoExercise(int exerciseId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                // Nếu chưa đăng nhập, chuyển hướng về danh sách khóa học
                return RedirectToAction("Index", "Course");
            }

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

            // Tải lại danh sách câu hỏi từ cơ sở dữ liệu nếu Questions trống
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

            // Kiểm tra đáp án
            var results = model.Questions.Select(q =>
            {
                var correctAnswer = q.Answers.FirstOrDefault(a => a.IsCorrect);
                var selectedAnswerId = model.UserAnswers.ContainsKey(q.QuestionId) ? model.UserAnswers[q.QuestionId] : 0;

                return new
                {
                    QuestionId = q.QuestionId,
                    IsCorrect = correctAnswer?.AnswerId == selectedAnswerId
                };
            }).ToList();
            var totalQuestions = model.Questions.Count;
            var correctAnswers = model.Questions.Count(q =>
                q.Answers.Any(a => a.AnswerId == model.UserAnswers[q.QuestionId] && a.IsCorrect));


            var score = (decimal)correctAnswers / totalQuestions * 10;  // Ví dụ: điểm số tối đa là 10
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
                Console.WriteLine("dsad" + scoreRequest.ExerciseId, scoreRequest.DateCompleted, scoreRequest.Score, scoreRequest.IsPassed);

                Console.WriteLine("Lưu điểm số thất bại.");
                ViewBag.Error = "Lưu điểm số thất bại.";
                return RedirectToAction("Index", "Exercise");
            }
            // Trả kết quả về View
            model.Submitted = true;
            return View("Results", model);
        }
        [HttpPost]
        // Xử lý tạo bài tập
        public async Task<IActionResult> Create([FromBody] ExerciseRequestWrapper model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid course data.sadsad" });
                }
                var result = await _exerciseService.AddExerciseAsync(model);

                // Console.WriteLine(course.CourseName+course.CategoryID+course.Description+course.InstructorID);
                if (result.IsSuccessed != false)
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
