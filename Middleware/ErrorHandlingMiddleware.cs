using System.Net;

namespace PatientManagment.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private IWebHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExecptionAsync(context, ex, _env);
            }
        }
        public static Task HandleExecptionAsync(HttpContext httpContext, Exception ex, IWebHostEnvironment env)
        {
            Exception exception = ex.InnerException ?? ex;
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";
            var response = new
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = exception.Message,
                Details = env.IsProduction() ? string.Empty : exception.StackTrace
            };
            return httpContext.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }
    }
}
