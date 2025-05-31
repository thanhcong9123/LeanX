using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Courses;

namespace LearnX_ApiIntegration
{
    public interface ICourseApiClient
    {
        Task<List<Course>> GetAll();
        Task<Course> GetbyID(Guid id);
        Task<bool> Create(CourseRequest course);
        Task<bool> Delete(int id);
        Task<bool> Edit(Guid id, Course course);
        Task<MyCourses> GetmyCourse(Guid id);
        Task<List<AppUser>> GetUserCourse(int idCourse);
    }
}