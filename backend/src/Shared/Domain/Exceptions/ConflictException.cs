namespace Shared.Domain.Exceptions;

public sealed class ConflictException(
    string message,
    string? code = null) : AppException(message, code)
{
}