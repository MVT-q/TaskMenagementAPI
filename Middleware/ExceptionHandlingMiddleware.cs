using TaskMenagementAPI.Exceptions;

namespace TaskMenagementAPI.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var statusCode = ex switch
                {
                    UserAlreadyExistsException => StatusCodes.Status409Conflict,

                    CurrentUserNotFoundException => StatusCodes.Status400BadRequest,

                    AccessDeniedException => StatusCodes.Status403Forbidden,

                    _ => StatusCodes.Status500InternalServerError
                };

                context.Response.StatusCode = statusCode;

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    message = ex.Message
                });
            }
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(
            this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
