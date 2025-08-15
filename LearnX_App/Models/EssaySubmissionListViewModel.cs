using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;

namespace LearnX_App.Models
{
    public class EssaySubmissionListViewModel
    {
        public List<EssaySubmissions>? Submissions { get; set; }
        public int ExerciseId { get; set; }
        public string? ExerciseTitle { get; set; }
        public bool IsTeacher { get; set; } = false;
        public bool ShowAllSubmissions { get; set; } = false; // Hiển thị tất cả submission hay chỉ của user hiện tại
    }
}
