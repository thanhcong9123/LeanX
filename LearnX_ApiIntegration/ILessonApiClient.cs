using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Lessons;
using LearnX_ModelView.Common;

namespace LearnX_ApiIntegration
{
    public interface ILessonApiClient
    {
        Task<List<Lesson>> GetAllLessonsAsync(int courseId);
        Task<bool> AddLessonAsync(LessonRequest lesson);
        Task<bool> DeleteLessonAsync(int lessonId);
    }
}