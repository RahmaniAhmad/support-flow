namespace Shared.Contracts.Storage;

public sealed class AttachmentStorageException(
    string operation,
    string? storageErrorCode = null,
    int? status = null,
    Exception? innerException = null)
    : Exception($"Attachment storage {operation} failed.", innerException)
{
    public string Operation { get; } = operation;

    public string? StorageErrorCode { get; } = storageErrorCode;

    public int? Status { get; } = status;
}
