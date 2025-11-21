using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LearnX_Data.EF;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Category;
using Microsoft.EntityFrameworkCore;

namespace LearnX_Application.Comman
{
    public class CategoryService : ICategoryService
    {
        public LearnXDbContext _context;
        private readonly IMapper _mapper;
        public CategoryService(LearnXDbContext context, IMapper mapper)
        {
            _context =context;
            _mapper = mapper;
        }
        public async Task<int> Create(CategoryRequest category)
        {
            var data = _mapper.Map<Category>(category);
            await _context.Categories.AddAsync(data);
            await _context.SaveChangesAsync();
            return data.CategoryID;

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