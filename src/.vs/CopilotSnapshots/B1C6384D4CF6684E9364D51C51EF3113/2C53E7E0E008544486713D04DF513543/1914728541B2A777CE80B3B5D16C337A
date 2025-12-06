using App.Core.Exceptions.Commons;
using App.Core.Exceptions;
using Newtonsoft.Json;

namespace App.API.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
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
                await HandleException(context, ex);
            }
        }

        private Task HandleException(HttpContext context, Exception ex)
        {
            var code = StatusCodes.Status500InternalServerError;
            var errors = new List<string> { ex.Message };

            switch (ex)
            {
                case EntityNotFoundException:
                    code = StatusCodes.Status404NotFound;
                    break;
                case UnauthorizedException:
                case UnauthorizedAccessException:
                    code = StatusCodes.Status401Unauthorized;
                    break;
                case BadRequestException:
                    code = StatusCodes.Status400BadRequest;
                    break;
            }

            var result = JsonConvert.SerializeObject(new { success = false, errors });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = code;

            return context.Response.WriteAsync(result);
        }
    }
}
