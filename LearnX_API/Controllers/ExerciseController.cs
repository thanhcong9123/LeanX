using LearnX_Application.Comman;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Exercise;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetExercises()
        {
            var exercises = await _exerciseService.GetAllExercisesAsync();
            return Ok(exercises);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExercise(int id)
        {
            var exercise = await _exerciseService.GetExerciseByIdAsync(id);
            if (exercise == null) return NotFound();
            return Ok(exercise);
        }
        [HttpGet("exercise/{idCourse}")]
        public async Task<IActionResult> GetExerciseforCourse(int idCourse)
        {
            var exercise = await _exerciseService.GetExerciseByIdcourseAsync(idCourse);
            return Ok(exercise);
        }
        [HttpGet("question/{idCourse}")]
        public async Task<IActionResult> GetQuestionforExercise(int idCourse)
        {
            var exercise = await _exerciseService.GetQuestionByIdExerciseAsync(idCourse);
            return Ok(exercise);
        }

        [HttpPost]
        public async Task<IActionResult> AddExercise(ExerciseRequestWrapper wrapper)
        {
            var exerciseId = await _exerciseService.AddExerciseAsync(wrapper);
            return CreatedAtAction(nameof(GetExercise), new { id = exerciseId }, wrapper.ExerciseRequest);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateExercise(int id, [FromForm] ExerciseRequest exerciseRequest)
        {
            if (exerciseRequest.ExerciseId != id) return BadRequest();

            await _exerciseService.UpdateExerciseAsync(exerciseRequest);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id)
        {
            await _exerciseService.DeleteExerciseAsync(id);
            return NoContent();
        }
        [HttpGet("user/{userId}/exercises")]
        public async Task<ActionResult<List<Exercise>>> GetExercisesForUser(Guid userId)
        {
            var exercises = await _exerciseService.GetExercisesForUserAsync(userId);

            if (exercises == null || !exercises.Any())
            {
                return Ok(null);
            }

            return Ok(exercises);
        }
       
         [HttpPost("submit")]
        public async Task<ActionResult> SubmitExercise(SubmitExerciseRequest request)
        {
            var reponse = await _exerciseService.SubmitExerciseAsync(request.Questions, request.UserId, request.ExerciseId);
            
            if (reponse == null)
            {
                return BadRequest("Submission failed.");
            }
            return Ok(reponse);
        }
    }

}
