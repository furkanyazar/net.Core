using System.Net.Mime;
using System.Text.Json;
using Core.CrossCuttingConcerns.Exception.WebApi.Handlers;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Logging.Abstraction;
using Microsoft.AspNetCore.Http;

namespace Core.CrossCuttingConcerns.Exception.WebApi.Middleware;

public class ExceptionMiddleware(
    RequestDelegate next,
    IHttpContextAccessor contextAccessor,
    ILogger loggerService
    )
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
    private readonly HttpExceptionHandler _httpExceptionHandler = new HttpExceptionHandler();
    private readonly ILogger _loggerService = loggerService;
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (System.Exception exception)
        {
            await LogException(context, exception);
            await HandleExceptionAsync(context.Response, exception);
        }
    }

    protected virtual Task HandleExceptionAsync(HttpResponse response, dynamic exception)
    {
        response.ContentType = MediaTypeNames.Application.Json;
        _httpExceptionHandler.Response = response;

        return _httpExceptionHandler.HandleException(exception);
    }

    protected virtual Task LogException(HttpContext context, System.Exception exception)
    {
        List<LogParameter> logParameters =
        [
            new LogParameter { Type = context.GetType().Name, Value = exception.ToString() },
        ];

        LogDetail logDetail = new()
        {
            MethodName = _next.Method.Name,
            Parameters = logParameters,
            User = _contextAccessor.HttpContext?.User.Identity?.Name ?? "?",
        };

        _loggerService.Information(JsonSerializer.Serialize(logDetail));
        return Task.CompletedTask;
    }
}
