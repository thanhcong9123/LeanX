using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.EF;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Exercise;
using LearnX_Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LearnX_Application.Comman
{
    public class ExerciseService : IExerciseService
    {
        private readonly LearnXDbContext _context;

        public ExerciseService(LearnXDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ExerciseRequest>> GetAllExercisesAsync()
        {
            // Truy xuất dữ liệu từ DB và tải vào bộ nhớ
            var exercises = await _context.Exercises
                .Include(e => e.Questions)
                .ThenInclude(q => q.Answers)
                .ToListAsync();

            // Ánh xạ sang DT
            return exercises.Select(e => new ExerciseRequest
            {
                ExerciseId = e.ExerciseId,
                Title = e.Title,
                CourseId = e.CourseId,
                // Questions = e.Questions?.Select(q => new QuestionRequest
                // {
                //     QuestionId = q.QuestionId,
                //     QuestionText = q.QuestionText,
                //     Answers = q.Answers?.Select(a => new AnswerRequest
                //     {
                //         AnswerId = a.AnswerId,
                //         AnswerText = a.AnswerText,
                //         IsCorrect = a.IsCorrect
                //     }).ToList()
                // }).ToList()
            });
        }

        public async Task<ExerciseRequest?> GetExerciseByIdAsync(int id)
        {
            var exercise = await _context.Exercises
                .Include(e => e.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(e => e.ExerciseId == id);

            if (exercise == null) return null;

            return new ExerciseRequest
            {
                ExerciseId = exercise.ExerciseId,
                Title = exercise.Title,
                CourseId = exercise.CourseId,
                // Questions = exercise.Questions?.Select(q => new QuestionRequest
                // {
                //     QuestionId = q.QuestionId,
                //     QuestionText = q.QuestionText,
                //     Answers = q.Answers?.Select(a => new AnswerRequest
                //     {
                //         AnswerId = a.AnswerId,
                //         AnswerText = a.AnswerText,
                //         IsCorrect = a.IsCorrect
                //     }).ToList()
                // }).ToList()
            };
        }

        public async Task<int> AddExerciseAsync(ExerciseRequest exerciseRequest, List<QuestionRequest> questionRequest)
        {

            // Tạo mới Exercise
            var exercise = new Exercise
            {
                Title = exerciseRequest.Title,
                CourseId = exerciseRequest.CourseId,
            };

            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            // Kiểm tra nếu Questions không null
            if (questionRequest != null)
            {
                foreach (var item in questionRequest)
                {
                    Console.WriteLine("code chạy" + item.QuestionText);
                    // Tạo mới Question
                    var question = new Question
                    {
                        QuestionText = item.QuestionText,
                        ExerciseId = exercise.ExerciseId
                    };

                    _context.Questions.Add(question);
                    await _context.SaveChangesAsync();

                    // Kiểm tra nếu Answers không null
                    if (item.Answers != null)
                    {
                        foreach (var item2 in item.Answers)
                        {
                            // Tạo mới Answer
                            var answer = new Answer
                            {
                                AnswerText = item2.AnswerText,
                                QuestionId = question.QuestionId,
                                IsCorrect = item2.IsCorrect
                            };

                            _context.Answers.Add(answer);
                        }

                        await _context.SaveChangesAsync();
                    }
                }
            }

            return exercise.ExerciseId;
        }


        public async Task<int> UpdateExerciseAsync(ExerciseRequest exerciseDto)
        {
            var existingExercise = await _context.Exercises
                .Include(e => e.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(e => e.ExerciseId == exerciseDto.ExerciseId);

            if (existingExercise == null)
                throw new MyClassException($"Exercise with ID {exerciseDto.ExerciseId} not found.");

            existingExercise.Title = exerciseDto.Title;
            existingExercise.CourseId = exerciseDto.CourseId;

            // Update Questions
            // existingExercise.Questions = exerciseDto.Questions?.Select(q => new Question
            // {
            //     QuestionId = q.QuestionId,
            //     QuestionText = q.QuestionText,
            //     Answers = q.Answers?.Select(a => new Answer
            //     {
            //         AnswerId = a.AnswerId,
            //         AnswerText = a.AnswerText,
            //         IsCorrect = a.IsCorrect
            //     }).ToList()
            // }).ToList();

            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteExerciseAsync(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);
            if (exercise == null)
                throw new MyClassException($"Exercise with ID {id} not found.");

            _context.Exercises.Remove(exercise);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Exercise>> GetExerciseByIdcourseAsync(int id)
        {
            return await _context.Exercises.Where(c => c.CourseId == id).ToListAsync();
        }

        public async Task<List<Question>> GetQuestionByIdExerciseAsync(int id)
        {
            return await _context.Questions.Where(q => q.ExerciseId == id).Include(m => m.Answers).ToListAsync();
        }
        public async Task<List<Exercise>> GetExercisesForUserAsync(Guid userId)
        {
            // Tìm tất cả các bài tập liên quan đến các khóa học mà người dùng đã đăng ký
            var exercises = await _context.Enrollments
                .Where(e => e.UserID == userId)  // Lọc các enrollment của người dùng
                .SelectMany(e => e.Course.Exercises)  // Lấy các bài tập của các khóa học mà người dùng đã tham gia
                .ToListAsync();  // Chuyển đổi thành danh sách

            return exercises;
        }
    }
}