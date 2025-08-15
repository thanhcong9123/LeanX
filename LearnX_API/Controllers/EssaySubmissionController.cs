using LearnX_Application.Comman;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.EssaySubmission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnX_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EssaySubmissionController : ControllerBase
    {
        private readonly IEssaySubmissionService _essaySubmissionService;

        public EssaySubmissionController(IEssaySubmissionService essaySubmissionService)
        {
            _essaySubmissionService = essaySubmissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEssaySubmissions()
        {
            var submissions = await _essaySubmissionService.GetAllEssaySubmissionsAsync();
            return Ok(submissions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEssaySubmission(int id)
        {
            var submission = await _essaySubmissionService.GetEssaySubmissionByIdAsync(id);
            if (submission == null) return NotFound();
            return Ok(submission);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetEssaySubmissionsByUser(Guid userId)
        {
            var submissions = await _essaySubmissionService.GetEssaySubmissionsByUserAsync(userId);
            return Ok(submissions);
        }

        [HttpGet("exercise/{exerciseId}")]
        public async Task<IActionResult> GetEssaySubmissionsByExercise(int exerciseId)
        {
            var submissions = await _essaySubmissionService.GetEssaySubmissionsByExerciseAsync(exerciseId);
            return Ok(submissions);
        }

        [HttpGet("user/{userId}/exercise/{exerciseId}")]
        public async Task<IActionResult> GetEssaySubmissionsByUserAndExercise(Guid userId, int exerciseId)
        {
            var submissions = await _essaySubmissionService.GetEssaySubmissionsByUserAndExerciseAsync(userId, exerciseId);
            return Ok(submissions);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEssaySubmission(CreateEssaySubmissionRequest request)
        {
            try
            {
                var submissionId = await _essaySubmissionService.CreateEssaySubmissionAsync(request);
                return CreatedAtAction(nameof(GetEssaySubmission), new { id = submissionId }, request);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEssaySubmission(int id, UpdateEssaySubmissionRequest request)
        {
            if (request.Id != id) return BadRequest("ID mismatch");

            try
            {
                await _essaySubmissionService.UpdateEssaySubmissionAsync(request);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEssaySubmission(int id)
        {
            try
            {
                await _essaySubmissionService.DeleteEssaySubmissionAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            try
            {
                await _essaySubmissionService.UpdateStatusAsync(id, status);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/comment")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddTeacherComment(int id, [FromForm] string comment)
        {
            try
            {
                await _essaySubmissionService.AddTeacherCommentAsync(id, comment);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
