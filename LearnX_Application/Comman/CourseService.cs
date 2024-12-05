using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.EF;
using LearnX_Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearnX_Application.Comman
{
    public class CourseService : ICourseService
    {
        public LearnXDbContext _context;
        public CourseService(LearnXDbContext context)
        {
            _context =context;
        }
        public async Task<int> CreateCourse(Course course)
        {

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return course.CourseID;
        }

        public Task<int> DeleteCourse(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Course>> GetMyCourse(Guid idUser)
        {
            var list = await _context.Courses.Where(n=>n.InstructorID == idUser).ToListAsync();
            return list;
        }
        public async Task<List<Course>> GetCourseSinged(Guid idUser)
        {
            var list = await _context.Courses.Where(n=>n.InstructorID == idUser).ToListAsync();
            return list;
        }

        public async Task<Course> GetByID(int id)
        {
            var course =  await _context.Courses.FindAsync(id);
            return course;
        }

        public Task<int> UpdateCourse(Course course)
        {
            throw new NotImplementedException();
        }
    }
}