using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Exercise;

namespace LearnX_Application.Comman
{
    public interface IExerciseService
    {
        Task<IEnumerable<ExerciseRequest>> GetAllExercisesAsync();
        Task<ExerciseRequest?> GetExerciseByIdAsync(int id);
        Task<List<Exercise>> GetExerciseByIdcourseAsync(int id);
        Task<List<Question>> GetQuestionByIdExerciseAsync(int id);
        Task<int> AddExerciseAsync(ExerciseRequest exerciseRequest,List<QuestionRequest> questionRequest);
        Task<int> UpdateExerciseAsync(ExerciseRequest exerciseDto);
        Task<int> DeleteExerciseAsync(int id);
        Task<List<Exercise>> GetExercisesForUserAsync(Guid userId);
    }
}