using System.Security.Claims;
using LearnX_ApiIntegration;
using LearnX_App.Models;
using LearnX_ModelView.Catalog.EssaySubmission;
using LearnX_ModelView.Catalog.Exercise;
using Microsoft.AspNetCore.Mvc;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;

namespace LearnX_App.Controllers
{
    public class EssaySubmissionController : Controller
    {
        private readonly IEssaySubmissionApiClient _essaySubmissionService;
        private readonly IExerciseApiClient _exerciseService;
        private readonly ICourseApiClient _courseService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Cloudinary _cloudinary; // add field and inject in ctor

        [ActivatorUtilitiesConstructor]
        public EssaySubmissionController(
            IEssaySubmissionApiClient essaySubmissionService,
            IExerciseApiClient exerciseService,
            ICourseApiClient courseService,
            IWebHostEnvironment webHostEnvironment,
            Cloudinary cloudinary)
        {
            _essaySubmissionService = essaySubmissionService;
            _exerciseService = exerciseService;
            _courseService = courseService;
            _webHostEnvironment = webHostEnvironment;
            _cloudinary = cloudinary;
        }

        // GET: Tạo bài tập tự luận mới
        public IActionResult CreateEssayExercise(int CourseId)
        {

            var model = new CreateEssayExerciseViewModel
            {
                CourseId = CourseId
            };

            return View(model);
        }

        // POST: Tạo bài tập tự luận
        [HttpPost]
        public async Task<IActionResult> CreateEssayExercise(CreateEssayExerciseViewModel model, string? AnswerKeyCloudUrl, string? AnswerKeyPublicId)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                // Nếu client đã upload trực tiếp lên Cloudinary sẽ gửi AnswerKeyCloudUrl
                string? answerKeyUrl = AnswerKeyCloudUrl;
                string? answerKeyPublicId = AnswerKeyPublicId;

                // Nếu client không upload trực tiếp, bạn có thể fallback to server-side upload (see option 2)

                var exerciseRequest = new ExerciseRequest
                {
                    Title = model.Title,
                    CourseId = model.CourseId,
                    Category = "Tự Luận",
                    AnswerFile = answerKeyUrl
                };

                var exerciseWrapper = new ExerciseRequestWrapper
                {
                    ExerciseRequest = exerciseRequest,
                    QuestionRequest = new List<QuestionRequest>() // empty = essay
                };

                var result = await _exerciseService.AddExerciseAsync(exerciseWrapper);

                if (result.IsSuccessed)
                {
                    // Save answer key metadata to your API/service if you need to link it to the exercise
                    if (!string.IsNullOrEmpty(answerKeyUrl))
                    {
                        // example: call EBook/Resource API to save link or extend Exercise model
                        // await _exerciseService.AddAnswerKeyToExerciseAsync(result.ResultObj, answerKeyUrl, answerKeyPublicId);
                    }

                    TempData["Success"] = "Tạo bài tập tự luận thành công!";
                    return RedirectToAction("Details", "Exercise", new { id = model.CourseId });
                }

                TempData["Error"] = result.Message ?? "Có lỗi xảy ra khi tạo bài tập.";
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
                return View(model);
            }
        }

        // GET: Hiển thị form làm bài tự luận
        public async Task<IActionResult> DoEssayExercise(int exerciseId)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var userGuid = Guid.Parse(userId);

            // Lấy thông tin exercise
            var exercise = await _exerciseService.GetExerciseByIdAsync(exerciseId);
            if (exercise == null)
            {
                TempData["Error"] = "Không tìm thấy bài tập.";
                return RedirectToAction("Index", "Course");
            }

            // Lấy thông tin bài tập hiện có của user
            var existingSubmissions = await _essaySubmissionService.GetEssaySubmissionsByUserAndExerciseAsync(userGuid, exerciseId);

            var viewModel = new DoEssayExerciseViewModel
            {
                ExerciseId = exerciseId,
                ExerciseTitle = exercise.Title,
                AttemptNumber = existingSubmissions?.Count + 1 ?? 1,
                ExistingSubmissions = existingSubmissions?.Select(s => new EssaySubmissionRequest
                {
                    Id = s.Id,
                    StudentAnswer = s.StudentAnswer,
                    AttachmentFilePath = s.AttachmentFilePath,
                    SubmittedAt = s.SubmittedAt,
                    Status = s.Status,
                    TeacherComment = s.TeacherComment,
                    AttemptNumber = s.AttemptNumber
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: Nộp bài tự luận
        [HttpPost]
        public async Task<IActionResult> SubmitEssayExercise(DoEssayExerciseViewModel model)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var userGuid = Guid.Parse(userId);
            string? fileName = null;

            // Xử lý upload file nếu có
            if (model.AttachmentFile != null && model.AttachmentFile.Length > 0)
            {
                try
                {
                    // Tạo thư mục upload nếu chưa có
                    var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "essays");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // Tạo tên file unique
                    fileName = $"{userGuid}_{model.ExerciseId}_{DateTime.Now:yyyyMMddHHmmss}_{model.AttachmentFile.FileName}";
                    var filePath = Path.Combine(uploadPath, fileName);

                    // Lưu file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.AttachmentFile.CopyToAsync(stream);
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Lỗi khi upload file: {ex.Message}";
                    return View("DoEssayExercise", model);
                }
            }

            // Tạo request để gửi lên API
            var request = new CreateEssaySubmissionRequest
            {
                IdUser = userGuid,
                ExerciseId = model.ExerciseId,
                StudentAnswer = model.StudentAnswer,
                AttachmentFileName = fileName,
                AttemptNumber = model.AttemptNumber
            };

            try
            {
                var result = await _essaySubmissionService.CreateEssaySubmissionAsync(request);

                if (result.IsSuccessed)
                {
                    TempData["Success"] = "Nộp bài thành công!";
                    return RedirectToAction("EssaySubmissionResult", new { exerciseId = model.ExerciseId, attemptNumber = model.AttemptNumber });
                }
                else
                {
                    TempData["Error"] = result.Message ?? "Có lỗi xảy ra khi nộp bài.";
                    return View("DoEssayExercise", model);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
                return View("DoEssayExercise", model);
            }
        }

        // GET: Hiển thị kết quả sau khi nộp bài
        public async Task<IActionResult> EssaySubmissionResult(int exerciseId, int attemptNumber)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var userGuid = Guid.Parse(userId);

            // Lấy thông tin exercise để hiển thị
            var exercise = await _exerciseService.GetExerciseByIdAsync(exerciseId);
            if (exercise == null)
            {
                TempData["Error"] = "Không tìm thấy bài tập.";
                return RedirectToAction("Index", "Course");
            }

            var viewModel = new DoEssayExerciseViewModel
            {
                ExerciseId = exerciseId,
                ExerciseTitle = exercise.Title,
                AttemptNumber = attemptNumber,
                IsSubmitted = true,
                SubmissionResult = "Bài của bạn đã được nộp thành công và đang chờ giáo viên chấm điểm."
            };

            return View(viewModel);
        }

        // GET: Xem danh sách bài nộp của exercise
        public async Task<IActionResult> ViewSubmissions(int exerciseId, bool showAll = false)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var userGuid = Guid.Parse(userId);

            // Lấy thông tin exercise để tìm courseId
            var exercise = await _exerciseService.GetExerciseByIdAsync(exerciseId);
            if (exercise == null)
            {
                TempData["Error"] = "Không tìm thấy bài tập.";
                return RedirectToAction("Index", "Course");
            }

            // Kiểm tra quyền tương tự như ExerciseController
            var courseofUser = await _courseService.GetmyCourse(userGuid);
            if (courseofUser == null)
            {
                return RedirectToAction("Index", "Course");
            }

            var isInMyCourse = courseofUser.MyCourse?.Any(c => c.CourseID == exercise.CourseId) ?? false;

            if (!isInMyCourse)
            {
                TempData["Error"] = "Bạn không có quyền truy cập vào bài tập này.";
                return RedirectToAction("Index", "Course");
            }
            // Truyền ViewBag để View sử dụng
            ViewBag.isInMyCourse = isInMyCourse;

            var submissions = await _essaySubmissionService.GetEssaySubmissionsByExerciseAsync(exerciseId)
                ;

            var viewModel = new EssaySubmissionListViewModel
            {
                Submissions = submissions,
                ExerciseId = exerciseId,
                ShowAllSubmissions = showAll && isInMyCourse, // Chỉ cho phép showAll nếu là chủ sở hữu
                IsTeacher = isInMyCourse // Thay đổi logic kiểm tra
                
            };

            return View(viewModel);
        }

        // GET: Xem chi tiết bài nộp
        public async Task<IActionResult> ViewSubmissionDetail(int id)
        {
            var submission = await _essaySubmissionService.GetEssaySubmissionByIdAsync(id);
            if (submission == null)
            {
                TempData["Error"] = "Không tìm thấy bài nộp.";
                return RedirectToAction("Index", "Exercise");
            }

            return View(submission);
        }

        // POST: Giáo viên thêm nhận xét
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTeacherComment(int id, string comment)
        {
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest"
                         || Request.Headers["Accept"].ToString().Contains("application/json");

            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                if (isAjax) return Json(new { success = false, message = "Không có quyền truy cập" });
                return RedirectToAction("Login", "Account");
            }

            var submission = await _essaySubmissionService.GetEssaySubmissionByIdAsync(id);
            if (submission == null)
            {
                if (isAjax) return Json(new { success = false, message = "Không tìm thấy bài nộp" });
                TempData["Error"] = "Không tìm thấy bài nộp.";
                return RedirectToAction("ViewSubmissionDetail", new { id });
            }

            var exercise = await _exerciseService.GetExerciseByIdAsync(submission.ExerciseId);
            if (exercise == null)
            {
                if (isAjax) return Json(new { success = false, message = "Không tìm thấy bài tập" });
                TempData["Error"] = "Không tìm thấy bài tập.";
                return RedirectToAction("ViewSubmissionDetail", new { id });
            }

            var userGuid = Guid.Parse(userId);
            var courseofUser = await _courseService.GetmyCourse(userGuid);
            var isOwner = courseofUser?.MyCourse?.Any(c => c.CourseID == exercise.CourseId) ?? false;

            if (!isOwner)
            {
                if (isAjax) return Json(new { success = false, message = "Bạn không có quyền thực hiện chức năng này" });
                TempData["Error"] = "Bạn không có quyền thực hiện chức năng này.";
                return RedirectToAction("ViewSubmissionDetail", new { id });
            }

            try
            {
                var result = await _essaySubmissionService.AddTeacherCommentAsync(id, comment ?? string.Empty);

                if (isAjax)
                {
                    return Json(new
                    {
                        success = result.IsSuccessed,
                        message = result.IsSuccessed ? "Thêm nhận xét thành công!" : (result.Message ?? "Có lỗi xảy ra khi thêm nhận xét")
                    });
                }

                if (result.IsSuccessed)
                {
                    TempData["Success"] = "Thêm nhận xét thành công!";
                }
                else
                {
                    TempData["Error"] = result.Message ?? "Có lỗi xảy ra khi thêm nhận xét.";
                }
            }
            catch (Exception ex)
            {
                if (isAjax) return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction("ViewSubmissionDetail", new { id });
        }

        // POST: Cập nhật trạng thái bài nộp
        [HttpPost]
        [Authorize(Roles ="Teacher")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
            // Kiểm tra quyền - phải là chủ sở hữu khóa học
            var submission = await _essaySubmissionService.GetEssaySubmissionByIdAsync(id);
            if (submission == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                    Request.ContentType?.Contains("multipart/form-data") == true)
                {
                    return Json(new { success = false, message = "Không tìm thấy bài nộp" });
                }
                TempData["Error"] = "Không tìm thấy bài nộp.";
                return RedirectToAction("ViewSubmissionDetail", new { id });
            }

            var exercise = await _exerciseService.GetExerciseByIdAsync(submission.ExerciseId);
            if (exercise == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                    Request.ContentType?.Contains("multipart/form-data") == true)
                {
                    return Json(new { success = false, message = "Không tìm thấy bài tập" });
                }
                TempData["Error"] = "Không tìm thấy bài tập.";
                return RedirectToAction("ViewSubmissionDetail", new { id });
            }

            var userGuid = Guid.Parse(userId);
            var courseofUser = await _courseService.GetmyCourse(userGuid);
            var isOwner = courseofUser?.MyCourse?.Any(c => c.CourseID == exercise.CourseId) ?? false;

            if (!isOwner)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                    Request.ContentType?.Contains("multipart/form-data") == true)
                {
                    return Json(new { success = false, message = "Bạn không có quyền thực hiện chức năng này" });
                }
                TempData["Error"] = "Bạn không có quyền thực hiện chức năng này.";
                return RedirectToAction("ViewSubmissionDetail", new { id });
            }

            try
            {
                var result = await _essaySubmissionService.UpdateStatusAsync(id, status);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                    Request.ContentType?.Contains("multipart/form-data") == true)
                {
                    if (result.IsSuccessed)
                    {
                        return Json(new { success = true, message = "Cập nhật trạng thái thành công!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = result.Message ?? "Có lỗi xảy ra khi cập nhật trạng thái" });
                    }
                }

                // For regular form submission
                if (result.IsSuccessed)
                {
                    TempData["Success"] = "Cập nhật trạng thái thành công!";
                }
                else
                {
                    TempData["Error"] = result.Message ?? "Có lỗi xảy ra khi cập nhật trạng thái.";
                }
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                    Request.ContentType?.Contains("multipart/form-data") == true)
                {
                    return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
                }
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToAction("ViewSubmissionDetail", new { id });
        }

        // GET: Download file đính kèm
        public IActionResult DownloadAttachment(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound();
            }

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "essays", fileName);

            if (!System.IO.File.Exists(filePath))
            {
                TempData["Error"] = "File không tồn tại.";
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }
    }
}
