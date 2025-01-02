using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_ModelView.Catalog.Exercise;

namespace LearnX_Application.Comman
{
    public interface IExerciseService
    {
        Task<IEnumerable<ExerciseRequest>> GetAllExercisesAsync();
        Task<ExerciseRequest?> GetExerciseByIdAsync(int id);
        Task<int> AddExerciseAsync(ExerciseRequest exerciseDto);
        Task<int> UpdateExerciseAsync(ExerciseRequest exerciseDto);
        Task<int> DeleteExerciseAsync(int id);
    }
}