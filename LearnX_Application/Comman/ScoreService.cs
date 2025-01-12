using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.EF;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Exercise;
using LearnX_ModelView.Catalog.Scores;
using Microsoft.EntityFrameworkCore;

namespace LearnX_Application.Comman
{
    public class ScoreService : IScoreService
    {
        public LearnXDbContext _context;
        public ScoreService(LearnXDbContext context)
        {
            _context = context;
        }

        public async Task<int> Create(ScoreRequest courseRequest)
        {
            Console.WriteLine("dsad" + courseRequest.IsPassed);
            var score1 = await _context.Scores.FirstOrDefaultAsync(n => n.IdUser == courseRequest.IdUser && n.ExerciseId == courseRequest.ExerciseId);
            if (score1 != null)
            {
                _context.Scores.Remove(score1);

                await _context.SaveChangesAsync();
            }
            var course = new Scores()
            {
                IdUser = courseRequest.IdUser,
                ExerciseId = courseRequest.ExerciseId,
                DateCompleted = courseRequest.DateCompleted,
                Score = courseRequest.Score,
                IsPassed = courseRequest.IsPassed
            };
            Console.WriteLine(courseRequest.IdUser + "Id uer     ");
            await _context.Scores.AddAsync(course);
            await _context.SaveChangesAsync();
            return course.Id;
        }

        public async Task<List<ExerciseScoreDto>> GetAll(Guid idUser)
        {
            var result = await (from score in _context.Scores
                                join exercise in _context.Exercises on score.ExerciseId equals exercise.ExerciseId
                                where score.IdUser == idUser
                                select new ExerciseScoreDto
                                {
                                    ExerciseId = exercise.ExerciseId,
                                    ExerciseTitle = exercise.Title,
                                    Score = score.Score,
                                    IsPassed = score.IsPassed,
                                    DateCompleted = score.DateCompleted
                                }).ToListAsync();

            return result;
        }
    }
}