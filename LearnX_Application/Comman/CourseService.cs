using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.EF;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Courses;
using LearnX_Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LearnX_Application.Comman
{
    public class CourseService : ICourseService
    {
        public LearnXDbContext _context;
        public CourseService(LearnXDbContext context)
        {
            _context = context;
        }
        public async Task<int> CreateCourse(CourseRequest course)
        {
            Course course1 = new Course()
            {
                CourseName = course.CourseName,
                Description = course.Description,
                InstructorID = course.InstructorID,
                CategoryID = course.CategoryID,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                Price = course.Price
            };
            await _context.Courses.AddAsync(course1);
            await _context.SaveChangesAsync();
            return course1.CourseID;
        }

        public async Task<int> DeleteCourse(int id)
        {
            var course = await _context.Courses
                              .Include(c => c.Enrollments) // Load related enrollments
                              .FirstOrDefaultAsync(c => c.CourseID == id);

            if (course == null)
            {
                throw new MyClassException($"Cannot find a course with ID: {id}");
            }

            // Xóa tất cả các bản ghi trong Enrollments liên quan đến Course
            if (course.Enrollments != null && course.Enrollments.Any())
            {
                _context.Enrollments.RemoveRange(course.Enrollments);
            }

            // Xóa bản ghi Course
            _context.Courses.Remove(course);

            // Lưu thay đổi
            return await _context.SaveChangesAsync();
        }

        public async Task<List<Course>> GetMyCourse(Guid idUser)
        {
            var list = await _context.Courses.Where(n => n.InstructorID == idUser).ToListAsync();
            return list;
        }
        public async Task<List<Course>> GetCourseSinged(Guid idUser)
        {
            var courses = await _context.Enrollments
                    .Where(e => e.UserID == idUser)
                    .Include(e => e.Course) // Bao gồm thông tin khóa học
                    .Select(e => e.Course) // Lấy đối tượng Course
                    .ToListAsync();
            return courses;
        }

        public async Task<Course> GetByID(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            return course;
        }

        public Task<int> UpdateCourse(CourseUpdateRequest course)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AppUser>> GetCourseUser(int id)
        {
             var students = await _context.Enrollments
            .Where(e => e.CourseID == id) // Tìm các enrollment có CourseID giống với khóa học
            .Include(e => e.User) // Liên kết với AppUser
            .Select(e => e.User).ToListAsync();
            return students;
        }
    }
}