using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Lessons;

namespace LearnX_Application.Comman
{
    public interface ILessonService
    {
        Task<IEnumerable<Lesson>> GetAllLessonsAsync(int idcourse);
        Task<Lesson?> GetLessonByIdAsync(int id);
        Task<int> AddLessonAsync(LessonRequest lesson);
        Task<int> UpdateLessonAsync(LessonRequest lesson);
        Task<int> DeleteLessonAsync(int id);
        
    }
}