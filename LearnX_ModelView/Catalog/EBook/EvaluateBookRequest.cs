using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.EBook
{
    public class EvaluateBookRequest
    {
         public int BookId { get; set; }
        public Guid UserId { get; set; }
        public float Rating { get; set; }
        public string Comment { get; set; }
    }
}