using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Exercise;
using LearnX_ModelView.Catalog.Scores;
using LearnX_ModelView.Common;

namespace LearnX_ApiIntegration
{
    public interface IScoreApiClient
    {
        Task<List<ExerciseScoreDto>> GetScoreAsync(Guid userId);
        Task<bool> AddScoreAsync(ScoreRequest scoreRequest);
    }
}