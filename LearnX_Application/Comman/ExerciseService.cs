using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        public ExerciseService(LearnXDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                Category = e.Category,
                AnswerFile = e.AnswerFile,
                Describe = e.Describe,
                Instruct = e.Instruct

            });
        }

        public async Task<Exercise> GetExerciseByIdAsync(int id)
        {
            var exercise = await _context.Exercises
                .AsNoTracking()
                .Include(e => e.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(e => e.ExerciseId == id);

            if (exercise == null) return null;
            if (exercise.Questions != null)
            {
                foreach (var q in exercise.Questions)
                {
                    if (q.Answers == null) continue;
                    foreach (var a in q.Answers)
                    {
                        a.IsCorrect = false;
                    }
                }
            }

            return exercise;
        }

        public async Task<int> AddExerciseAsync(ExerciseRequestWrapper exerciseRequest)
        {

            // Tạo mới Exercise
            if (exerciseRequest == null) throw new ArgumentNullException(nameof(exerciseRequest));

            var exercise = _mapper.Map<Exercise>(exerciseRequest);
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
            existingExercise.Category = exerciseDto.Category;
            existingExercise.AnswerFile = exerciseDto.AnswerFile;

            existingExercise.Describe = exerciseDto.Describe;
            existingExercise.Instruct = exerciseDto.Instruct;

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
            return await _context.Exercises.Include(e=> e.Questions).ThenInclude(q=>q.Answers).Where(c => c.CourseId == id)
                                            .ToListAsync();
        }

        public async Task<List<Question>> GetQuestionByIdExerciseAsync(int id)
        {
            return await _context.Questions.Where(q => q.ExerciseId == id).Include(m => m.Answers).ToListAsync();
        }
        public async Task<List<Exercise>> GetExercisesForUserAsync(Guid userId)
        {
          
            var exercises = await _context.Enrollments
                .Where(e => e.UserID == userId) 
                .SelectMany(e => e.Course.Exercises).Include(e => e.Questions).Include(e => e.Course)
                .ToListAsync();  

            return exercises;
        }
        public async Task<int> SubmitExerciseAsync(List<QuestionSumit> questions, Guid userId, int exerciseId)
        {
            int score = 0;
            foreach (var q in questions)
            {
                var chekck = await _context.Answers
                    .Where(a => a.QuestionId == q.QuestionId && a.AnswerId == q.SelectedAnswerId && a.IsCorrect == true)
                    .FirstOrDefaultAsync();
                if (chekck != null)
                {
                    score++;
                }
            }
            var countQuestion = await _context.Questions.Where(q => q.ExerciseId == exerciseId).CountAsync();
            if(countQuestion == 0 || score == 0 ) return 0;
            var result =  (score * 10) / countQuestion;
            return result;
        }
       
    }
}