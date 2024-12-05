using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace LearnX_Utilities.Exceptions
{
    public class MyClassException: Exception
    {
         public MyClassException()
        {
        }

        public MyClassException(string message)
            : base(message)
        {
        }

        public MyClassException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}