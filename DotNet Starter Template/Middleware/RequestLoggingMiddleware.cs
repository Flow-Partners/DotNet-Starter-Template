using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DotNet_Starter_Template.Middleware
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
            var correlationId = Guid.NewGuid().ToString();
            context.Items["CorrelationId"] = correlationId;

            var stopwatch = Stopwatch.StartNew();
            var request = await FormatRequest(context.Request);

            _logger.LogInformation("Request started: {Method} {Path} {QueryString} {Headers} {Body} {CorrelationId}",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                FormatHeaders(context.Request.Headers),
                request,
                correlationId);

            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation("Request completed: {StatusCode} {ElapsedMilliseconds}ms {CorrelationId}",
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                correlationId);
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            body.Position = 0;
            request.Body = body;

            return bodyAsText;
        }

        private string FormatHeaders(IHeaderDictionary headers)
        {
            var headerString = new StringBuilder();
            foreach (var header in headers)
            {
                // Skip sensitive headers
                if (IsSensitiveHeader(header.Key))
                {
                    headerString.Append($"{header.Key}: [REDACTED] ");
                }
                else
                {
                    headerString.Append($"{header.Key}: {header.Value} ");
                }
            }
            return headerString.ToString();
        }

        private bool IsSensitiveHeader(string headerName)
        {
            var sensitiveHeaders = new[] { "authorization", "cookie", "x-api-key", "x-auth-token" };
            return sensitiveHeaders.Contains(headerName.ToLower());
        }
    }
}
