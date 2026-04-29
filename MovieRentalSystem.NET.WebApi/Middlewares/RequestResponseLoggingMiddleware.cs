using System.Diagnostics;

namespace MovieRentalSystem.NET.WebApi.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;


        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                await _next(context);
            }
            finally
            {
                sw.Stop();

                _logger.LogInformation(
                    "HTTP {Method} {Path} => {StatusCode} in {Elapsed} ms", context.Request?.Method,
                    context.Request?.Path.Value,
                    context.Response?.StatusCode,
                    sw.ElapsedMilliseconds);
            }
        }
    }
}
