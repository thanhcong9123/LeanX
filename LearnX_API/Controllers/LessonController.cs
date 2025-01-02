using LearnX_Application.Comman;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Lessons;
using LearnX_Utilities.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _context;

        public LessonController(ILessonService context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetLessons()
        {
            var lessons = await _context.GetAllLessonsAsync();
            return Ok(lessons);
        }

        // GET: api/Lesson/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLesson(int id)
        {
            var lesson = await _context.GetLessonByIdAsync(id);
            if (lesson == null) return NotFound(new { Message = $"Lesson with ID {id} not found." });
            return Ok(lesson);
        }

        // POST: api/Lesson
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddLesson([FromForm] LessonRequest lesson)
        {
            if (lesson == null)
                return BadRequest(new { Message = "Invalid lesson data." });

            try
            {
                var createdId = await _context.AddLessonAsync(lesson);
                return CreatedAtAction(nameof(GetLesson), new { id = createdId }, lesson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        // PUT: api/Lesson/{id}
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateLesson(int id, [FromForm] LessonRequest lesson)
        {
            if (lesson == null || lesson.LessonID != id)
                return BadRequest(new { Message = "Lesson data is invalid or mismatched with the ID." });

            try
            {
                await _context.UpdateLessonAsync(lesson);
                return NoContent();
            }
            catch (MyClassException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        // DELETE: api/Lesson/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            try
            {
                await _context.DeleteLessonAsync(id);
                return NoContent();
            }
            catch (MyClassException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
