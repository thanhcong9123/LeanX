using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Exercise
{
    public class AIGenerateQuestionsRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Range(1, 20)]
        public int NumberOfQuestions { get; set; } =  5;
    }
}