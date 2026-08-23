namespace Api.Exceptions;

public sealed class UnauthorizedException(
    string message,
    string? code = null)
    : AppException(message, code);