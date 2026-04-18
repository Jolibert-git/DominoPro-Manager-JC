

namespace Homework2.MidleWare
{
    public class LoggerAccessMidleware
    {
        private RequestDelegate next;

        public LoggerAccessMidleware(RequestDelegate _next)
        {
            this.next = _next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/boqueado")// If Route is /bloquedo
            {
                context.Response.StatusCode = 403;     // access denegate
                await context.Response.WriteAsync("Access denegate");//
            }
            else
            {
                await next.Invoke(context);
            }

        }
    }

    public static class LoggerAccesMidlewareExtention
    {
        public static IApplicationBuilder UseLoggerAccess(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggerAccessMidleware>();
        }
    }
}
