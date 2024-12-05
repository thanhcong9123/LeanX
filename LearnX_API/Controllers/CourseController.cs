using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LearnX_Data.EF;
using LearnX_Data.Entities;
using LearnX_Application.Comman;

namespace LearnX_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _context;

        public CourseController(ICourseService context)
        {
            _context = context;
        }

        // GET: api/Course/
        [HttpGet("{iduser}")]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses(Guid iduser)
        {
            return Ok(await _context.GetMyCourse(iduser));
        }

        // GET: api/Course/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.GetByID(id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        // PUT: api/Course/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.CourseID)
            {
                return BadRequest();
            }


       

            return NoContent();
        }

        // POST: api/Course
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Consumes("multipart/form-data")]

        public async Task<ActionResult<Course>> PostCourse([FromBody]Course course)
        {
         
            if(ModelState.IsValid!)
            {
                Console.WriteLine(course);
                return BadRequest(ModelState);
            }
            var result=  await _context.CreateCourse(course);
            if(result == 0)
            {
                return BadRequest();
            }
            var createdCouse = _context.GetByID(result);
            return CreatedAtAction("GetCourse", new { id = result }, course);
        }

        // DELETE: api/Course/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
           

            return NoContent();
        }

        
    }
}
