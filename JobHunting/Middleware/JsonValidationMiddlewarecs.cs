using System.Text;
using System.Text.Json;

namespace AppMiddlewarExample.Middleware
{
    public class JsonValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public JsonValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            if (context.Request.ContentType != null &&
                context.Request.ContentType.Contains("application/json"))
            {
                try
                {
                    context.Request.EnableBuffering();

                    using (var reader = new StreamReader(context.Request.Body,
                        Encoding.UTF8, true, 1024, true))
                    {
                        var body = await reader.ReadToEndAsync();
                        //await Console.Out.WriteLineAsync(body);

                        JsonSerializer.Deserialize(body, typeof(object));
                        context.Request.Body.Position = 0;
                        await _next.Invoke(context);
                    }

                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Bad JSON data");
                    throw new Exception("Bad JSON data");
                }
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
