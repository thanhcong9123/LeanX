using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.EssaySubmission
{
    public class EssaySubmissionRequest
    {
        public int Id { get; set; }
        public Guid IdUser { get; set; }
        public int ExerciseId { get; set; }
        public string? StudentAnswer { get; set; }
        public string? AnswerKeyFilePath { get; set; }
        public string? AttachmentFilePath { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string? Status { get; set; } = "Submitted";
        public string? TeacherComment { get; set; }
        public int AttemptNumber { get; set; } = 1;
    }
}
