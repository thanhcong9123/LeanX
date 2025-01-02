using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using LearnX_Data.EF;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Lessons;
using LearnX_Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LearnX_Application.Comman
{
    public class LessonService : ILessonService
    {
        private readonly LearnXDbContext _context;

        public LessonService(LearnXDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lesson>> GetAllLessonsAsync()
        {
            return await _context.Lessons.ToListAsync();
        }

        public async Task<Lesson?> GetLessonByIdAsync(int id)
        {
            return await _context.Lessons.FirstOrDefaultAsync(l => l.LessonID == id);
        }

        public async Task<int> AddLessonAsync(LessonRequest lesson)
        {
            Lesson lesson1 = new Lesson(){
                CourseID = lesson.CourseID,
                LessonTitle = lesson.LessonTitle,
                Content = lesson.Content
            };
            _context.Lessons.Add(lesson1);
            await _context.SaveChangesAsync();
            return lesson.LessonID;
        }

        public async Task<int> UpdateLessonAsync(LessonRequest lesson)
        {
            var existingLesson = await _context.Lessons.FindAsync(lesson.LessonID);
            if (existingLesson == null) throw new MyClassException($"Cannot find a course with ID: {lesson.LessonID}");;

            existingLesson.LessonTitle = lesson.LessonTitle;
            existingLesson.Content = lesson.Content;
            existingLesson.CourseID = lesson.CourseID;
            return await _context.SaveChangesAsync(); ;
        }

        public async Task<int> DeleteLessonAsync(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null)  throw new MyClassException($"Cannot find a course with ID: {id}");;

            _context.Lessons.Remove(lesson);

            return await _context.SaveChangesAsync(); ;
        }
    }


}