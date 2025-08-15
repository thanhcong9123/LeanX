using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Evaluation;
using AutoMapper;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Lessons;
using LearnX_ModelView.Catalog.Exercise;

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


        }
    }

}