using System.Diagnostics;

namespace UserManagementApi.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var method = context.Request.Method;
            var path = context.Request.Path;

            _logger.LogInformation("[REQUEST] -> {Method} {Path}", method, path);

            await _next(context);

            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;

            _logger.LogInformation("[RESPONSE] <- {Method} {Path} devolvió {StatusCode} en {Elapsed} ms", 
                method, path, statusCode, stopwatch.ElapsedMilliseconds);
        }
    }
}

