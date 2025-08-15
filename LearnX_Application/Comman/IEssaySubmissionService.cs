using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.EssaySubmission;

namespace LearnX_Application.Comman
{
    public interface IEssaySubmissionService
    {
        Task<IEnumerable<EssaySubmissionRequest>> GetAllEssaySubmissionsAsync();
        Task<EssaySubmissionRequest?> GetEssaySubmissionByIdAsync(int id);
        Task<List<EssaySubmissions>> GetEssaySubmissionsByUserAsync(Guid userId);
        Task<List<EssaySubmissions>> GetEssaySubmissionsByExerciseAsync(int exerciseId);
        Task<List<EssaySubmissions>> GetEssaySubmissionsByUserAndExerciseAsync(Guid userId, int exerciseId);
        Task<int> CreateEssaySubmissionAsync(CreateEssaySubmissionRequest request);
        Task<int> UpdateEssaySubmissionAsync(UpdateEssaySubmissionRequest request);
        Task<int> DeleteEssaySubmissionAsync(int id);
        Task<int> UpdateStatusAsync(int id, string status);
        Task<int> AddTeacherCommentAsync(int id, string comment);
    }
}
