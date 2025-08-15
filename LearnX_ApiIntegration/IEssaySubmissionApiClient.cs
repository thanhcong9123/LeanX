using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.EssaySubmission;
using LearnX_ModelView.Common;

namespace LearnX_ApiIntegration
{
    public interface IEssaySubmissionApiClient
    {
        Task<ApiResult<string>> CreateEssaySubmissionAsync(CreateEssaySubmissionRequest request);
        Task<List<EssaySubmissions>> GetEssaySubmissionsByUserAsync(Guid userId);
        Task<List<EssaySubmissions>> GetEssaySubmissionsByExerciseAsync(int exerciseId);
        Task<List<EssaySubmissions>> GetEssaySubmissionsByUserAndExerciseAsync(Guid userId, int exerciseId);
        Task<EssaySubmissionRequest?> GetEssaySubmissionByIdAsync(int id);
        Task<ApiResult<string>> UpdateEssaySubmissionAsync(UpdateEssaySubmissionRequest request);
        Task<bool> DeleteEssaySubmissionAsync(int id);
        Task<ApiResult<string>> UpdateStatusAsync(int id, string status);
        Task<ApiResult<string>> AddTeacherCommentAsync(int id, string comment);
    }
}
