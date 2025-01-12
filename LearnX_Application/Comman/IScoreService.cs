using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Exercise;
using LearnX_ModelView.Catalog.Scores;

namespace LearnX_Application.Comman
{
    public interface IScoreService
    {
            Task<int> Create(ScoreRequest course);
            Task<List<ExerciseScoreDto>> GetAll(Guid idUser);

    }
}