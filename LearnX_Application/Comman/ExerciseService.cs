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
                Questions = e.Questions?.Select(q => new QuestionRequest
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
                Questions = exercise.Questions?.Select(q => new QuestionRequest
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
        }

        public async Task<int> AddExerciseAsync(ExerciseRequest exerciseDto)
        {
            var exercise = new Exercise
            {
                Title = exerciseDto.Title,
                CourseId = exerciseDto.CourseId,
                Questions = exerciseDto.Questions?.Select(q => new Question
                {
                    QuestionText = q.QuestionText,
                    Answers = q.Answers?.Select(a => new Answer
                    {
                        AnswerText = a.AnswerText,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList()
            };

            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();
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
            existingExercise.Questions = exerciseDto.Questions?.Select(q => new Question
            {
                QuestionId = q.QuestionId,
                QuestionText = q.QuestionText,
                Answers = q.Answers?.Select(a => new Answer
                {
                    AnswerId = a.AnswerId,
                    AnswerText = a.AnswerText,
                    IsCorrect = a.IsCorrect
                }).ToList()
            }).ToList();

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
    }
}