using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_ModelView.Catalog.Exercise;

namespace LearnX_Application.Comman.AI
{
    public interface IAIQuestionGenerator
    {
        Task<AIGeneratedQuestionsResponse> GenerateQuestionsAsync(AIGenerateQuestionsRequest request);
        Task<AIAutoScoringResponse> ScoringAsync(AIScoringRequest request);
    }
}