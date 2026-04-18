namespace Homework2.MidleWare
{
    public class LoggerRequestMidleware
    {
        public RequestDelegate next;

        public LoggerRequestMidleware(RequestDelegate _next)
        {
            this.next = _next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();//create logger 
            logger.LogInformation($"Answer: {context.Request.Method} {context.Request.Path}");//use logger for write method and route

            await next.Invoke(context); //continue code

            logger.LogInformation($"Answer: {context.Response.StatusCode}");// use logger for write Status example: 404,200... 
        }
    }

    public static class LoguearRequestMidlewareExtention
    {
        public static IApplicationBuilder UseLoggerRequest(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggerRequestMidleware>();
        }

    }
}
