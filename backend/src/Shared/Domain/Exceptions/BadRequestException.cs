namespace Shared.Domain.Exceptions;

public sealed class BadRequestException(
    string message,
    string? code = null) : AppException(message, code)
{
}
