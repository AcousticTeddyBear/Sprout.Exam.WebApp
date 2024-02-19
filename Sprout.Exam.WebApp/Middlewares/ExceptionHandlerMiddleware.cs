using Microsoft.AspNetCore.Http;
using Sprout.Exam.Common.Exceptions;
using Sprout.Exam.WebApp.Models;
using System;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ApiException ex)
            {
                await handleException(httpContext, ex, ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                await handleException(httpContext, ex);
            }
        }

        private async Task handleException(HttpContext httpContext, Exception ex,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string errMessage = "An error occurred while processing the request.")
        {
            //Logging can be added here
            Debug.WriteLine($"ERROR - {ex.GetType().Name} - {ex.Message}");
            httpContext.Response.Clear();
            httpContext.Response.StatusCode = (int)statusCode;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new ApiResponse
            {
                Message = errMessage
            }, new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
        }
    }
}
