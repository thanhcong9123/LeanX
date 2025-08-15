using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class Exercise
    {
        [Key]
        public int ExerciseId { get; set; }
        public string? Title { get; set; }

        // Một bài tập thuộc về một khóa học
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public string? Category { get; set; }
        public string? AnswerFile { get; set; }
        public string? Describe { get; set; }
        public string? Instruct { get; set; }

        // Một bài tập có nhiều câu hỏi
        public ICollection<Question>? Questions { get; set; }
        
        // Một bài tập có nhiều bài nộp tự luận
        public ICollection<EssaySubmissions>? EssaySubmissions { get; set; }
    }
}