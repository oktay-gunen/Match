using System.Net;

namespace Match.Web.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (HttpException e)
            {
                await HandleHttpExceptionAsync(httpContext, e);
            }
            catch (Exception ex)
            {
                await HandleUnhandledExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleUnhandledExceptionAsync(HttpContext context,
                                Exception exception)
        {
            _logger.LogError($"Unhandled Exception:{exception} ***** ExceptionMessage:{exception.Message} ***** StackTrace: {exception.StackTrace} ***** InnerException: {exception.InnerException}");

            if (!context.Response.HasStarted)
            {
                int statusCode = (int)HttpStatusCode.InternalServerError; // 500
                string message = string.Empty;
#if DEBUG
                message = exception.Message;
#else
                message =  "An unhandled exception has occurred";
#endif
                context.Response.Clear();
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var result = new ExceptionMessage(message).ToString();
                await context.Response.WriteAsync(result);
            }
        }

        private async Task HandleHttpExceptionAsync(HttpContext context, HttpException exception)
        {
            _logger.LogError($"Handled Exception:{exception} ***** ExceptionMessage:{exception.Message} ***** StackTrace: {exception.StackTrace} ***** InnerException: {exception.InnerException}");

            if (!context.Response.HasStarted)
            {
                int statusCode = exception.StatusCode;
                string message = exception.Message;

                context.Response.Clear();

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var result = new ExceptionMessage(message).ToString();
                await context.Response.WriteAsync(result);
            }
        }
    }
}