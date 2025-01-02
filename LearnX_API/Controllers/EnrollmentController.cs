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
using LearnX_ModelView.Catalog.Enrollment;

namespace LearnX_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _context;

        public EnrollmentController(IEnrollmentService context)
        {
            _context = context;
        }
        [HttpGet("enrollment/{id}")]
        public async Task<ActionResult<Course>> GetEnrollment(int id)
        {
            var enrollment = await _context.GetEnrollment(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return Ok(enrollment);
        }
        // GET: api/Enrollment

        // POST: api/Enrollment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Enrollment>> PostEnrollment([FromForm] EnrollmentRequest enrollment)
        {
            if (!ModelState.IsValid!)
            {
                return BadRequest(ModelState);
            }
            var result = await _context.Create(enrollment);
            if (result == 0)
            {
                return BadRequest();
            }
            return CreatedAtAction("GetEnrollment", new { id = result }, enrollment);
        }
        [HttpDelete]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Enrollment>> Outclass([FromForm] OutclassRequest enrollment)
        {
            if (!ModelState.IsValid!)
            {
                return BadRequest(ModelState);
            }
            var result = await _context.Outclass(enrollment.CourseID, enrollment.UserID);
            if (result == 0)
            {
                return BadRequest();
            }
            return CreatedAtAction("GetEnrollment", new { id = result }, enrollment);
        }

    }
}
