namespace Api_Gateway.Middlewares;

public class AuthMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower();

        if (path != null &&
            (path.Contains("/identity/login") || path.Contains("/identity/register")))
        {
            await next(context);
            return;
        }

        if (!context.Request.Cookies.TryGetValue("wasLogin", out var value) || value != "true")
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized. Please login.");
            return;
        }

        await next(context);
    }
}