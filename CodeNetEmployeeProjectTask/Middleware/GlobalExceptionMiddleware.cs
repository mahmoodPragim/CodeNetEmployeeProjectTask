using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using CodeNetEmployeeProjectTask.DTO;
using CodeNetEmployeeProjectTask.Exceptions;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly bool _isDevelopment;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _isDevelopment = environment.IsDevelopment();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Validation error occurred.");
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = new ApiResponse<object>
        {
            Status = "Error",
            Error = "An unexpected error occurred. Please try again later."
        };

        if (exception is NotFoundException)
        {
            code = HttpStatusCode.NotFound;
            result.Error = "Resource not found.";
        }
        else if (exception is BadRequestException)
        {
            code = HttpStatusCode.BadRequest;
            result.Error = "Bad request.";
        }
        else if (_isDevelopment)
        {
            result.Error = exception.ToString();
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsJsonAsync(result);
    }

    private Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        var code = HttpStatusCode.BadRequest;
        var details = exception.Errors.ToDictionary(
            e => e.PropertyName,
            e => (IEnumerable<string>)new List<string> { e.ErrorMessage }
        );
        var result = new ApiResponse<object>
        {
            Status = "Error",
            Error = "One or more validation errors occurred.",
            Details = details
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsJsonAsync(result);
    }

}
