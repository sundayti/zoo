using System.Net;
using System.Text.Json;

namespace Presentation.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _log;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> log)
    {
        _next = next;
        _log  = log;
    }

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Unhandled exception");
            await Handle(ctx, ex);
        }
    }

    private static Task Handle(HttpContext ctx, Exception ex)
    {
        var code = HttpStatusCode.InternalServerError;
        object body = new { error = "Internal Server Error" };

        if (ex is KeyNotFoundException)
        {
            code = HttpStatusCode.NotFound;
            body = new { error = ex.Message };
        }
        else if (ex is ArgumentException || ex is InvalidOperationException)
        {
            code = HttpStatusCode.BadRequest;
            body = new { error = ex.Message };
        }

        var json = JsonSerializer.Serialize(body);
        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = (int)code;
        return ctx.Response.WriteAsync(json);
    }
}