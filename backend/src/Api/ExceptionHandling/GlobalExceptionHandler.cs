using Api.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.ExceptionHandling;

public sealed class GlobalExceptionHandler
    : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        LogException(httpContext, exception);

        var problemDetails = CreateProblemDetails(
            httpContext,
            exception);

        httpContext.Response.StatusCode =
            problemDetails.Status
            ?? StatusCodes.Status500InternalServerError;

        httpContext.Response.ContentType =
            "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }

    private void LogException(
        HttpContext httpContext,
        Exception exception)
    {
        switch (exception)
        {
            case ValidationException:
            case BadRequestException:
            case UnauthorizedException:
            case ForbiddenException:
            case NotFoundException:
            case ConflictException:
                _logger.LogWarning(
                    "Request failed with {ExceptionType}. " +
                    "TraceId: {TraceId}",
                    exception.GetType().Name,
                    httpContext.TraceIdentifier);
                break;

            default:
                _logger.LogError(
                    exception,
                    "Unhandled exception occurred. " +
                    "TraceId: {TraceId}",
                    httpContext.TraceIdentifier);
                break;
        }
    }

    private static ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        Exception exception)
    {
        return exception switch
        {
            ValidationException validationException =>
                CreateValidationProblemDetails(
                    httpContext,
                    validationException),

            UnauthorizedException unauthorizedException =>
                CreateProblemDetails(
                    httpContext,
                    StatusCodes.Status401Unauthorized,
                    "Unauthorized",
                    unauthorizedException.Message,
                    unauthorizedException.Code),

            ForbiddenException forbiddenException =>
                CreateProblemDetails(
                    httpContext,
                    StatusCodes.Status403Forbidden,
                    "Forbidden",
                    forbiddenException.Message,
                    forbiddenException.Code),

            NotFoundException notFoundException =>
                CreateProblemDetails(
                    httpContext,
                    StatusCodes.Status404NotFound,
                    "Resource Not Found",
                    notFoundException.Message,
                    notFoundException.Code),

            ConflictException conflictException =>
                CreateProblemDetails(
                    httpContext,
                    StatusCodes.Status409Conflict,
                    "Conflict",
                    conflictException.Message,
                    conflictException.Code),

            BadRequestException badRequestException =>
                CreateProblemDetails(
                    httpContext,
                    StatusCodes.Status400BadRequest,
                    "Bad Request",
                    badRequestException.Message,
                    badRequestException.Code),

            _ =>
                CreateProblemDetails(
                    httpContext,
                    StatusCodes.Status500InternalServerError,
                    "Internal Server Error",
                    "An unexpected error occurred.")
        };
    }

    private static ProblemDetails CreateProblemDetails(
        HttpContext httpContext,
        int statusCode,
        string title,
        string detail,
        string? code = null)
    {
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] =
            httpContext.TraceIdentifier;

        if (!string.IsNullOrWhiteSpace(code))
        {
            problemDetails.Extensions["code"] = code;
        }

        return problemDetails;
    }

    private static ProblemDetails CreateValidationProblemDetails(
        HttpContext httpContext,
        ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Select(error => error.ErrorMessage)
                    .Distinct()
                    .ToArray());

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Validation Failed",
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["errors"] = errors;

        problemDetails.Extensions["traceId"] =
            httpContext.TraceIdentifier;

        return problemDetails;
    }
}