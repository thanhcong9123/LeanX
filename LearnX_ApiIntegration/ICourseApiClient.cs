using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;

namespace LearnX_ApiIntegration
{
    public interface ICourseApiClient
    {
        Task<List<Course>> GetAll();
        Task<Course> GetbyID(Guid id);
        Task<int> Create(Course course);
        Task<int> Delete(Guid id);
        Task<int> Edit(Guid id, Course course);
    }
}