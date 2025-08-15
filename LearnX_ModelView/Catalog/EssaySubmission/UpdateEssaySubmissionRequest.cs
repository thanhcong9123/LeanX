using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.EssaySubmission
{
    public class UpdateEssaySubmissionRequest
    {
        public int Id { get; set; }
        public string? StudentAnswer { get; set; }
        public string? AttachmentFileName { get; set; }
        public string? Status { get; set; }
        public string? TeacherComment { get; set; }
        public int AttemptNumber { get; set; }
    }
}
