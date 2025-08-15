using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_ModelView.Catalog.EssaySubmission;
using Microsoft.AspNetCore.Http;

namespace LearnX_App.Models
{
    public class DoEssayExerciseViewModel
    {
        public int ExerciseId { get; set; }
        public string? ExerciseTitle { get; set; }
        public string? StudentAnswer { get; set; }
        public IFormFile? AttachmentFile { get; set; }
        public string? AttachmentFileName { get; set; }
        public int AttemptNumber { get; set; } = 1;
        public bool IsSubmitted { get; set; } = false;
        public string? SubmissionResult { get; set; }
        
        // Để hiển thị bài nộp đã có (nếu có)
        public List<EssaySubmissionRequest>? ExistingSubmissions { get; set; }
        
        // Để hiển thị đáp án chuẩn (cho giáo viên)
        public string? AnswerKeyFilePath { get; set; }
        public bool IsTeacher { get; set; } = false;
    }
}
