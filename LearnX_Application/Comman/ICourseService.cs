using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;

namespace LearnX_Application.Comman
{
    public interface ICourseService
    {
        Task<List<Course>> GetMyCourse(Guid nameUser);
        Task<List<Course>> GetCourseSinged(Guid nameUser);
        Task<Course> GetByID(int id);
        Task<int> CreateCourse(Course course);
        Task<int> UpdateCourse(Course course);
        Task<int> DeleteCourse(Guid id);



    }
}