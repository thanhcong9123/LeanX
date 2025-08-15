using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.EF;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.EssaySubmission;
using LearnX_Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LearnX_Application.Comman
{
    public class EssaySubmissionService : IEssaySubmissionService
    {
        private readonly LearnXDbContext _context;

        public EssaySubmissionService(LearnXDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EssaySubmissionRequest>> GetAllEssaySubmissionsAsync()
        {
            var submissions = await _context.EssaySubmissions
                .Include(es => es.User)
                .Include(es => es.Exercise)
                .ToListAsync();

            return submissions.Select(es => new EssaySubmissionRequest
            {
                Id = es.Id,
                IdUser = es.IdUser,
                ExerciseId = es.ExerciseId,
                StudentAnswer = es.StudentAnswer,
                AttachmentFilePath = es.AttachmentFilePath,
                SubmittedAt = es.SubmittedAt,
                Status = es.Status,
                TeacherComment = es.TeacherComment,
                AttemptNumber = es.AttemptNumber
            });
        }

        public async Task<EssaySubmissionRequest?> GetEssaySubmissionByIdAsync(int id)
        {
            var submission = await _context.EssaySubmissions
                .Include(es => es.User)
                .Include(es => es.Exercise)
                .FirstOrDefaultAsync(es => es.Id == id);

            if (submission == null) return null;

            return new EssaySubmissionRequest
            {
                Id = submission.Id,
                IdUser = submission.IdUser,
                ExerciseId = submission.ExerciseId,
                StudentAnswer = submission.StudentAnswer,
                AttachmentFilePath = submission.AttachmentFilePath,
                SubmittedAt = submission.SubmittedAt,
                Status = submission.Status,
                TeacherComment = submission.TeacherComment,
                AttemptNumber = submission.AttemptNumber
            };
        }

        public async Task<List<EssaySubmissions>> GetEssaySubmissionsByUserAsync(Guid userId)
        {
            return await _context.EssaySubmissions
                .Where(es => es.IdUser == userId)
                .Include(es => es.Exercise)
                .OrderByDescending(es => es.SubmittedAt)
                .ToListAsync();
        }

        public async Task<List<EssaySubmissions>> GetEssaySubmissionsByExerciseAsync(int exerciseId)
        {
            return await _context.EssaySubmissions
                .Where(es => es.ExerciseId == exerciseId)
                .Include(es => es.User)
                .OrderByDescending(es => es.SubmittedAt)
                .ToListAsync();
        }

        public async Task<List<EssaySubmissions>> GetEssaySubmissionsByUserAndExerciseAsync(Guid userId, int exerciseId)
        {
            return await _context.EssaySubmissions
                .Where(es => es.IdUser == userId && es.ExerciseId == exerciseId)
                .OrderByDescending(es => es.AttemptNumber)
                .ToListAsync();
        }

        public async Task<int> CreateEssaySubmissionAsync(CreateEssaySubmissionRequest request)
        {
            // Kiểm tra xem Exercise có tồn tại không
            var exerciseExists = await _context.Exercises.AnyAsync(e => e.ExerciseId == request.ExerciseId);
            if (!exerciseExists)
                throw new MyClassException($"Exercise with ID {request.ExerciseId} not found.");

            // Kiểm tra xem User có tồn tại không
            var userExists = await _context.AppUsers.AnyAsync(u => u.Id == request.IdUser);
            if (!userExists)
                throw new MyClassException($"User with ID {request.IdUser} not found.");

            // Tạo đường dẫn file attachment nếu có
            string? attachmentPath = null;
            if (!string.IsNullOrEmpty(request.AttachmentFileName))
            {
                // File được lưu ở app, API chỉ lưu tên file
                attachmentPath = $"uploads/essays/{request.AttachmentFileName}";
            }

            var submission = new EssaySubmissions
            {
                IdUser = request.IdUser,
                ExerciseId = request.ExerciseId,
                StudentAnswer = request.StudentAnswer,
                AttachmentFilePath = attachmentPath,
                SubmittedAt = DateTime.Now,
                Status = "Submitted",
                AttemptNumber = request.AttemptNumber,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.EssaySubmissions.Add(submission);
            await _context.SaveChangesAsync();

            return submission.Id;
        }

        public async Task<int> UpdateEssaySubmissionAsync(UpdateEssaySubmissionRequest request)
        {
            var submission = await _context.EssaySubmissions.FindAsync(request.Id);
            if (submission == null)
                throw new MyClassException($"Essay submission with ID {request.Id} not found.");

            // Cập nhật các thông tin
            if (!string.IsNullOrEmpty(request.StudentAnswer))
                submission.StudentAnswer = request.StudentAnswer;

            if (!string.IsNullOrEmpty(request.AttachmentFileName))
                submission.AttachmentFilePath = $"uploads/essays/{request.AttachmentFileName}";

            if (!string.IsNullOrEmpty(request.Status))
                submission.Status = request.Status;

            if (!string.IsNullOrEmpty(request.TeacherComment))
                submission.TeacherComment = request.TeacherComment;

            if (request.AttemptNumber > 0)
                submission.AttemptNumber = request.AttemptNumber;

            submission.UpdatedAt = DateTime.Now;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteEssaySubmissionAsync(int id)
        {
            var submission = await _context.EssaySubmissions.FindAsync(id);
            if (submission == null)
                throw new MyClassException($"Essay submission with ID {id} not found.");

            _context.EssaySubmissions.Remove(submission);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateStatusAsync(int id, string status)
        {
            var submission = await _context.EssaySubmissions.FindAsync(id);
            if (submission == null)
                throw new MyClassException($"Essay submission with ID {id} not found.");

            submission.Status = status;
            submission.UpdatedAt = DateTime.Now;

            return await _context.SaveChangesAsync();
        }

        public async Task<int> AddTeacherCommentAsync(int id, string comment)
        {
            Console.WriteLine($"Adding comment to submission ID {id}: {comment}");
            var submission = await _context.EssaySubmissions.FindAsync(id);

            if (submission == null)
                throw new MyClassException($"Essay submission with ID {id} not found.");

            submission.TeacherComment = comment;
            submission.Status = "Graded";
            submission.UpdatedAt = DateTime.Now;

            return await _context.SaveChangesAsync();
        }
    }
}
