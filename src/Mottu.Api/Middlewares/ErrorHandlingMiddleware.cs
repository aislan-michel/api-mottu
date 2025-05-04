using System.Net;
using System.Text.Json;

using Mottu.Api.Application.Models;

namespace Mottu.Api.Middlewares;

public class ErrorHandlingMiddleware(
    RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado");
            await HandleExceptionAsync(context, ex);
        }

        if (context.Response.StatusCode == 401)
        {
            await HandleUnauthorizedAsync(context);
        }

        if (context.Response.StatusCode == 403)
        {
            await HandleForbiddenAsync(context);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = Result<string>.Fail("Houve um erro inesperado ao processar sua solicitação - middleware.");
        
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private Task HandleUnauthorizedAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

        var response = Result<string>.Fail("Você precisa se autenticar para acessar este recurso.");

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private Task HandleForbiddenAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        var response = Result<string>.Fail("Você não tem permissão para acessar este recurso.");

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

