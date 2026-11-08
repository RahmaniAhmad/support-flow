namespace Shared.Contracts.Storage;

public interface IAttachmentStorage
{
    Task UploadAsync(
        string storagePath,
        Stream content,
        string contentType,
        CancellationToken cancellationToken);

    Task<Stream> OpenReadAsync(
        string storagePath,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        string storagePath,
        CancellationToken cancellationToken);
}
