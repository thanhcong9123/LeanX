using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Exercise;
using LearnX_ModelView.Common;

namespace LearnX_ApiIntegration
{
    public interface IExerciseApiClient
    {
        Task<ApiResult<string>> AddExerciseAsync(ExerciseRequestWrapper model);
        Task<bool> DeleteExerciseAsync(int id);
        Task<List<Exercise>> GetAll(Guid id);
        Task<List<Exercise>> GetExerciseDetailsAsync(int courseId);
        Task<List<Question>> getQuestion(int id);
        Task<bool> IsEssayExercise(int exerciseId);
        Task<Exercise?> GetExerciseByIdAsync(int exerciseId);



    }
}