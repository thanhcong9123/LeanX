using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AopDemo
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        Console.WriteLine($"Request bắt đầu: {context.Request.Method} {context.Request.Path}");
        
        await _next(context);

        stopwatch.Stop();
        Console.WriteLine($"Request kết thúc. Thời gian: {stopwatch.ElapsedMilliseconds} ms");
    }
    }
}