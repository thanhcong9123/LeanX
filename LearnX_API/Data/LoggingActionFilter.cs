using System;

using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace AopDemo
{
    public class LoggingActionFilter : IActionFilter
    {
        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        // Trước khi action được thực thi
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} bắt đầu thực thi.");
            _logger.LogInformation($"Tham số: {string.Join(", ", context.ActionArguments.Select(kv => $"{kv.Key}: {kv.Value}"))}");
        }

        // Sau khi action hoàn thành
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"Action {context.ActionDescriptor.DisplayName} kết thúc.");
        }
    }
}