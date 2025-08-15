using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.EssaySubmission
{
    public class CreateEssaySubmissionRequest
    {
        public Guid IdUser { get; set; }
        public int ExerciseId { get; set; }
        public string? StudentAnswer { get; set; }
        public string? AttachmentFileName { get; set; } // Chỉ tên file, không phải đường dẫn đầy đủ
        public int AttemptNumber { get; set; } = 1;
    }
}
