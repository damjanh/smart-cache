using SmartCache.Validation;

namespace SmartCache.Middleware;

public class EmailValidation(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.RouteValues.TryGetValue("email", out var value))
        {
            var email = value?.ToString();

            if (!EmailValidator.IsValidEmail(email))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid email");
                return;
            }
        }

        await next(context);
    }
}