using LearnX_Application.Comman;
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

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddExercise([FromForm] ExerciseRequest exerciseRequest)
        {
            var exerciseId = await _exerciseService.AddExerciseAsync(exerciseRequest);
            return CreatedAtAction(nameof(GetExercise), new { id = exerciseId }, exerciseRequest);
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
    }
    
}
