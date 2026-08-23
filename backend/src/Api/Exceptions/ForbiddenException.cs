namespace Api.Exceptions;

public sealed class ForbiddenException(
    string message,
    string? code = null)
    : AppException(message, code);