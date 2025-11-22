using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using AutoMapper;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Lessons;
using LearnX_ModelView.Catalog.Exercise;
using LearnX_Data.Migrations;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace LearnX_API.Data.Handler
{
    public class AutoMapperHandler : Profile
    {
        public AutoMapperHandler()
        {
            CreateMap<Lesson, LessonRequest>().ReverseMap();

            CreateMap<ResourcesLesson, ResourcesLessonRequest>().ReverseMap();
            // ...existing code...
            CreateMap<Answer, AnswerRequest>().ReverseMap();
            CreateMap<Question, QuestionRequest>().ReverseMap();
            CreateMap<Exercise, ExerciseRequest>().ReverseMap();

            CreateMap<Exercise, ExerciseRequestWrapper>()
                .ForMember(d => d.ExerciseRequest, opt => opt.MapFrom(s => s))
                .ForMember(d => d.QuestionRequest, opt => opt.MapFrom(s => s.Questions));

            CreateMap<ExerciseRequestWrapper, Exercise>()
                .IncludeMembers(s => s.ExerciseRequest)
                .ForMember(d => d.Questions, opt => opt.MapFrom(s => s.QuestionRequest))
                .ForMember(d => d.Course, opt => opt.Ignore())
                .ForMember(d => d.EssaySubmissions, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    if (dest.Questions == null) return;
                    foreach (var q in dest.Questions)
                    {
                        q.Exercise = dest;
                        if (q.Answers == null) continue;
                        foreach (var a in q.Answers) a.Question = q;
                    }
                });
            // ...existing code...
            // COurse
            CreateMap<Course, LearnX_ModelView.Catalog.Courses.CourseRequest>().ReverseMap();
            //Ebook
            CreateMap<EBook, LearnX_ModelView.Catalog.EBook.EBookRequest>().ReverseMap();
            CreateMap<EvaluateBook, LearnX_ModelView.Catalog.EBook.EvaluateBookRequest>().ReverseMap();
            //Enrollment
            CreateMap<LearnX_Data.Entities.Enrollment, LearnX_ModelView.Catalog.Enrollment.EnrollmentRequest>().ReverseMap();
            //EssaySubmission
            CreateMap<LearnX_Data.Entities.EssaySubmissions, LearnX_ModelView.Catalog.EssaySubmission.CreateEssaySubmissionRequest>()
                      .ForMember(dest => dest.AttachmentFileName,
                                 opt => opt.MapFrom(src => string.IsNullOrEmpty(src.AttachmentFilePath) ? null : Path.GetFileName(src.AttachmentFilePath)))
                      .ReverseMap()
                      .ForMember(dest => dest.AttachmentFilePath,
                                 opt => opt.MapFrom(src => src.AttachmentFileName)); // nếu cần path đầy đủ, prepend folder ở đây
            //Score
            CreateMap<LearnX_Data.Entities.Scores, LearnX_ModelView.Catalog.Scores.ScoreRequest>().ReverseMap();
            //Category
            CreateMap<LearnX_Data.Entities.Category, LearnX_ModelView.Catalog.Category.CategoryRequest>().ReverseMap();
        }
    }

}