using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AopDemo
{
    public class MyService : IMyService
    {
        public void DoSomething()
        {
            Console.WriteLine("Đang thực hiện công việc...");
        }
    }
}