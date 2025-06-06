using LearnX_Application.Comman;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Scores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoreController : ControllerBase
    {
         private readonly IScoreService _context;

        public ScoreController(IScoreService context)
        {
            _context = context;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAll(Guid idcourse)
        {
            var score = await _context.GetAll(idcourse);
            return Ok(score);
        }
         [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddLesson([FromForm] ScoreRequest lesson)
        {
            if (lesson == null)
                return BadRequest(new { Message = "Invalid lesson data." });

            try
            {
                var createdId = await _context.Create(lesson);
                return CreatedAtAction(nameof(GetAll), new { id = createdId }, lesson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
