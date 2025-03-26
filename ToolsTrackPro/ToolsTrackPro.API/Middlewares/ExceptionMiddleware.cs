using System.Net;
using System.Text.Json;
using FluentValidation;
using Serilog;
using ToolsTrackPro.API.Models;

namespace ToolsTrackPro.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
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
                Log.Error(ex, $"An unhandled exception occurred. Request Path: ${context.Request.Path}, Message: ${ex.Message}, Stack: ${ex.StackTrace}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                ValidationException validationException => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "Validation errors occurred.",
                    Errors = validationException.Errors.Select(e => e.ErrorMessage).ToList()
                },
                KeyNotFoundException => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "Resource not found."
                },
                UnauthorizedAccessException => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Message = "Unauthorized access."
                },
                _ => new ErrorResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An unexpected error occurred."
                }
            };

            context.Response.StatusCode = response.StatusCode;
            var responseResult = new ApiResponse<object>("Error, " + response.Message, response.Errors);
            var jsonResponse = JsonSerializer.Serialize(responseResult);
            await context.Response.WriteAsync(jsonResponse);
        }
    }

    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
    }
}
