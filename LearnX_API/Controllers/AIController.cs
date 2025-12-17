using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Application.Comman.AI;
using LearnX_ModelView.Catalog.Exercise;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LearnX_API.Controllers
{
    [Route("[controller]")]
    public class AIController : Controller
    {
        private readonly IAIQuestionGenerator _aiQuestionGenerator;

        public AIController(IAIQuestionGenerator aiQuestionGenerator)
        {
            _aiQuestionGenerator = aiQuestionGenerator;
        }

        [HttpPost("GenerateQuestions")]
        public async Task<IActionResult> GenerateQuestions([FromBody] AIGenerateQuestionsRequest request)
        {
            try
            {
                var result = await _aiQuestionGenerator.GenerateQuestionsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost("Scoring")]
        public async Task<IActionResult> Scoring([FromBody] AIScoringRequest request)
        {
            try
            {
                var result = await _aiQuestionGenerator.ScoringAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        
    }
}