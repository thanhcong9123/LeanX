using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace AopDemo
{
    public class LoggingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine($"Phương thức {invocation.Method.Name} bắt đầu.");
            invocation.Proceed();
            Console.WriteLine($"Phương thức {invocation.Method.Name} kết thúc."); 
        }
    }
}