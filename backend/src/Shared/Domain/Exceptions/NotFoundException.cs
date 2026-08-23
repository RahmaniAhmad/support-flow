namespace Shared.Domain.Exceptions;

public sealed class NotFoundException(
    string message,
    string? code = null) : AppException(message, code)
{
    public static NotFoundException For<T>(
        Guid id,
        string? code = null)
    {
        return new NotFoundException(
            $"{typeof(T).Name} with id '{id}' was not found.", code);
    }
}