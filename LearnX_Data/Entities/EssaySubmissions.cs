using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class EssaySubmissions
    {
        [Key]
        public int Id { get; set; }
        public Guid IdUser { get; set; }
        public AppUser? User { get; set; }
        public int ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }
        public string? StudentAnswer { get; set; }


        public string? AttachmentFilePath { get; set; }

        public DateTime SubmittedAt { get; set; }

        public string? Status { get; set; } = "Submitted";

        public string? TeacherComment { get; set; }
        
        public int AttemptNumber { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
