using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Exercise
{
    public class AIScoringRequest
    {
        [Required]
        public string AnswerKeyUrl { get; set; }
        [Required]
        public string SubmissionFileUrl { get; set; }
        [Required]
        public string ScoringCriteria { get; set; }
        [Range(0, 1000)]
        public int MaxScore { get; set; } = 100;
        public int? ExerciseId { get; set; }
        public Guid? StudentId { get; set; } 
    }
}