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
using LearnX_ModelView.Catalog.Category;

namespace LearnX_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _context;

        public CategoryController(ICategoryService context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            return Ok(await _context.getAll());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.getById(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CategoryRequest category)
        {

            var createdId = await _context.Create(category);
            return CreatedAtAction("GetCategory", new { id = createdId }, category);
        }
    }
}
