using System.Diagnostics;

namespace Consume_API.Middlewares
{
    /// <summary>
    /// Middleware for logging request
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        readonly Stopwatch _stopwatch = new Stopwatch();
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                _stopwatch.Start();
                await _next(context);
            }
            finally
            {
                _stopwatch.Stop();
                _logger.LogInformation(context.Request?.Path.Value, context.Request?.Method, _stopwatch.ElapsedMilliseconds);
                _logger.LogInformation(
                    "*Request {method} {url} => {statusCode}",
                    context.Request?.Method,
                    context.Request?.Path.Value,
                    context.Response?.StatusCode);
            }
        }
    }
}