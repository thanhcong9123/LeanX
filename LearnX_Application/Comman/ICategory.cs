using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;

namespace LearnX_Application.Comman
{
    public interface ICategory
    {
        Task<List<Category>> getAll();
        Task<int> Create(Category category);
        Task<Category> getById(int id);


    }
}