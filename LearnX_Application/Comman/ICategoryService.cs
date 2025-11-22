using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Category;

namespace LearnX_Application.Comman
{
    public interface ICategoryService
    {
        Task<List<Category>> getAll();
        Task<int> Create(CategoryRequest category);
        Task<Category> getById(int id);


    }
}