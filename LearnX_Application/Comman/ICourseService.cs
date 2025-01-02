using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Courses;

namespace LearnX_Application.Comman
{
    public interface ICourseService
    {
        Task<List<Course>> GetMyCourse(Guid nameUser);
        Task<List<Course>> GetCourseSinged(Guid nameUser);
        Task<Course> GetByID(int id);
        Task<int> CreateCourse(CourseRequest course);
        Task<int> UpdateCourse(CourseUpdateRequest course);
        Task<int> DeleteCourse(int id);
        



    }
}