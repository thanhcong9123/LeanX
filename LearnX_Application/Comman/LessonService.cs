using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using LearnX_Application.Base;
using LearnX_Data.EF;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Lessons;
using LearnX_Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace LearnX_Application.Comman
{
    public class LessonService : EntityBaseRepository<Lesson>, ILessonService
    {
        private readonly LearnXDbContext _context;
        private readonly IMapper _mapper;

        public LessonService(LearnXDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Lesson>> GetAllLessonsAsync(int idcourse)
        {
            return await _context.Lessons.Where(n => n.CourseID == idcourse)
            .Include(l => l.Resources)
            .Include(l => l.Course)  
            .ToListAsync();
        }

        public async Task<Lesson?> GetLessonByIdAsync(int id)
        {
            return await _context.Lessons.FirstOrDefaultAsync(l => l.LessonID == id);
        }

        public async Task<int> AddLessonAsync(LessonRequest lesson)
        {
            var lesson1 = _mapper.Map<Lesson>(lesson);
            _context.Lessons.Add(lesson1);
            await _context.SaveChangesAsync();
          
            return lesson.LessonID;
        }

        public async Task<int> UpdateLessonAsync(LessonRequest lesson)
        {
            var existingLesson = await _context.Lessons.FindAsync(lesson.LessonID);
            if (existingLesson == null) throw new MyClassException($"Cannot find a course with ID: {lesson.LessonID}"); ;

            existingLesson.LessonTitle = lesson.LessonTitle;
            existingLesson.Content = lesson.Content;
            existingLesson.CourseID = lesson.CourseID;
            return await _context.SaveChangesAsync(); ;
        }

        public async Task<int> DeleteLessonAsync(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) throw new MyClassException($"Cannot find a course with ID: {id}"); ;
            _context.Lessons.Remove(lesson);
            return await _context.SaveChangesAsync(); ;
        }
    }


}