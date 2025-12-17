using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Exercise
{
    public class AIAutoScoringResponse
    {
        public int Score { get; set; }

     
        public double Percentage { get; set; }


        public string Feedback { get; set; }

        public List<string> Weaknesses { get; set; } = new List<string>();

        public List<string> Strengths { get; set; } = new List<string>();

        public string Improvement { get; set; }

        public string Status { get; set; } = "Completed";

 
        public DateTime ScoredAt { get; set; } = DateTime.UtcNow;
    }
}