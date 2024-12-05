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

        // Một bài tập có nhiều câu hỏi
        public ICollection<Question>? Questions { get; set; }
    }
}