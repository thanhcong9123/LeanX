using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class EvaluateBook
    {
        public Guid Id { get; set; }
        public int BookId { get; set; }
        public EBook Book { get; set; }
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } 
        
        
    }
}