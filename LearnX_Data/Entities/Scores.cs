using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class Scores
    {
        public int Id { get; set; }
        public Guid IdUser { get; set; }

        public int ExerciseId { get; set; }

        public DateTime DateCompleted { get; set; }

        public decimal Score { get; set; }
        public bool IsPassed { get; set; }

    }
}