using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.EF;
using LearnX_Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnX_Application.Comman
{
    public class CategoryService : ICategory
    {
        public LearnXDbContext _context;
        public CategoryService(LearnXDbContext context)
        {
            _context =context;
        }
        public async Task<int> Create(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category.CategoryID;

        }

        public async Task<List<Category>> getAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> getById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            return category;
        }
    }
}