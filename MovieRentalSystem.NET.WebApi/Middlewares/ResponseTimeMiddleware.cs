using System.Diagnostics;

namespace MovieRentalSystem.NET.WebApi.Middlewares
{
    public class ResponseTimeMiddleware : IMiddleware
    {
        private readonly ILogger<ResponseTimeMiddleware> _logger;
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _defaultThreshold = TimeSpan.FromMilliseconds(500);
        public ResponseTimeMiddleware(ILogger<ResponseTimeMiddleware> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var watch = Stopwatch.StartNew();
            await next.Invoke(context);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            var thresholdConfigValue = _configuration.GetValue<string>("Middlewares:RequestTimeWarningThreshold");

            var threshold = TimeSpan.TryParse(thresholdConfigValue, out var parsedThreshold) ? parsedThreshold : _defaultThreshold;

            if (elapsedMs > threshold.TotalMilliseconds)
            {
                _logger.LogWarning("Request [{Method}] {Path} took {ElapsedMilliseconds} ms", context.Request.Method, context.Request.Path, elapsedMs);
            }
        }
    }
}
