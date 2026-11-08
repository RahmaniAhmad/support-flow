using Api.ExceptionHandling;
using Api.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Shared.Contracts.Storage;

namespace UnitTests.ExceptionHandling;

public sealed class GlobalExceptionHandlerTests
{
    [Fact]
    public async Task Validation_errors_return_bad_request()
    {
        var statusCode = await HandleAsync(
            new ValidationException("Invalid attachment."));

        Assert.Equal(StatusCodes.Status400BadRequest, statusCode);
    }

    [Theory]
    [InlineData(true, StatusCodes.Status401Unauthorized)]
    [InlineData(false, StatusCodes.Status403Forbidden)]
    public async Task Authorization_errors_return_authorization_status(
        bool unauthorized,
        int expectedStatusCode)
    {
        Exception exception = unauthorized
            ? new UnauthorizedException("Not authenticated.")
            : new ForbiddenException("Not permitted.");

        var statusCode = await HandleAsync(exception);

        Assert.Equal(expectedStatusCode, statusCode);
    }

    [Fact]
    public async Task Storage_errors_return_sanitized_service_unavailable()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var handled = await new GlobalExceptionHandler(
                NullLogger<GlobalExceptionHandler>.Instance)
            .TryHandleAsync(
                context,
                new AttachmentStorageException(
                    "upload",
                    "InvalidHeaderValue",
                    400,
                    new Exception("Sensitive emulator response.")),
                CancellationToken.None);

        context.Response.Body.Position = 0;
        using var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();

        Assert.True(handled);
        Assert.Equal(
            StatusCodes.Status503ServiceUnavailable,
            context.Response.StatusCode);
        Assert.DoesNotContain("Sensitive emulator response.", responseBody);
        Assert.Contains("ATTACHMENT_STORAGE_UNAVAILABLE", responseBody);
    }

    private static async Task<int> HandleAsync(Exception exception)
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await new GlobalExceptionHandler(
                NullLogger<GlobalExceptionHandler>.Instance)
            .TryHandleAsync(context, exception, CancellationToken.None);

        return context.Response.StatusCode;
    }
}
