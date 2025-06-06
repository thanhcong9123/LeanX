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
using LearnX_ModelView.Catalog.Courses;

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
        [HttpGet("user/{iduser}")]
        public async Task<ActionResult<MyCourses>> GetCourseForUser(Guid iduser)
        {
            var mycourse = await _context.GetMyCourse(iduser);
            var courseSinged = await _context.GetCourseSinged(iduser);
            var data = new MyCourses()
            {
                MyCourse = mycourse,
                CourseSinged = courseSinged
            };

            return Ok(data);
        }
        [HttpGet("courseUser/{idCourse}")]
        public async Task<ActionResult> GetCourseUser(int idCourse)
        {
            var courseSinged = await _context.GetCourseUser(idCourse);
          

            return Ok(courseSinged);
        }


        // GET: api/Course/5
        [HttpGet("course/{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _context.GetByID(id);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
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
        public async Task<ActionResult<Course>> PostCourse([FromForm] CourseRequest course)
        {

            if (!ModelState.IsValid!)
            {
                Console.WriteLine("không nhận đưucoj dữ liệu");
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return BadRequest(ModelState);
            }
                Console.WriteLine($"CourseName: {course.CourseName}, Description: {course.Description}, InstructorID: {course.InstructorID}, CategoryID: {course.CategoryID}, StartDate: {course.StartDate}, EndDate: {course.EndDate}, Price: {course.Price}");

            var result = await _context.CreateCourse(course);
            if (result == 0)
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
            var affectedResult = await _context.DeleteCourse(id);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }


    }
}
