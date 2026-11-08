using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Storage;

namespace Infrastructure.Storage;

public sealed class AzureBlobAttachmentStorage : IAttachmentStorage
{
    private const string ContainerName = "supportflow-attachments";

    private readonly BlobContainerClient _container;
    private readonly ILogger<AzureBlobAttachmentStorage> _logger;

    public AzureBlobAttachmentStorage(
        IConfiguration configuration,
        ILogger<AzureBlobAttachmentStorage> logger)
    {
        var connectionString =
            configuration.GetConnectionString("BlobStorage");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "The BlobStorage connection string is not configured.");
        }

        _container = new BlobContainerClient(
            connectionString,
            ContainerName,
            new BlobClientOptions(
                BlobClientOptions.ServiceVersion.V2023_11_03));
        _logger = logger;
    }

    public async Task UploadAsync(
        string storagePath,
        Stream content,
        string contentType,
        CancellationToken cancellationToken)
    {
        try
        {
            await EnsureContainerAsync(cancellationToken);

            var blob = _container.GetBlobClient(storagePath);
            await blob.UploadAsync(
                content,
                new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = contentType
                    }
                },
                cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (OperationCanceledException exception)
        {
            throw CreateStorageException("upload", exception);
        }
        catch (RequestFailedException exception)
        {
            throw CreateStorageException("upload", exception);
        }
        catch (Exception exception) when (IsStorageFailure(exception))
        {
            throw CreateStorageException("upload", exception);
        }
    }

    public async Task<Stream> OpenReadAsync(
        string storagePath,
        CancellationToken cancellationToken)
    {
        try
        {
            await EnsureContainerAsync(cancellationToken);

            return await _container
                .GetBlobClient(storagePath)
                .OpenReadAsync(cancellationToken: cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (OperationCanceledException exception)
        {
            throw CreateStorageException("read", exception);
        }
        catch (RequestFailedException exception)
        {
            throw CreateStorageException("read", exception);
        }
        catch (Exception exception) when (IsStorageFailure(exception))
        {
            throw CreateStorageException("read", exception);
        }
    }

    public async Task DeleteAsync(
        string storagePath,
        CancellationToken cancellationToken)
    {
        try
        {
            await EnsureContainerAsync(cancellationToken);

            var result = await _container
                .GetBlobClient(storagePath)
                .DeleteIfExistsAsync(
                    cancellationToken: cancellationToken);

            if (!result.Value)
            {
                throw new FileNotFoundException(
                    "The attachment blob does not exist.");
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (OperationCanceledException exception)
        {
            throw CreateStorageException("delete", exception);
        }
        catch (RequestFailedException exception)
        {
            throw CreateStorageException("delete", exception);
        }
        catch (Exception exception) when (IsStorageFailure(exception))
        {
            throw CreateStorageException("delete", exception);
        }
    }

    private static bool IsStorageFailure(Exception exception) =>
        exception is HttpRequestException or
            IOException or
            TimeoutException;

    private AttachmentStorageException CreateStorageException(
        string operation,
        Exception exception)
    {
        var storageException = exception as RequestFailedException;

        _logger.LogError(
            "Attachment storage {Operation} failed. Status: {Status}; Error code: {ErrorCode}; Exception type: {ExceptionType}.",
            operation,
            storageException?.Status,
            storageException?.ErrorCode,
            exception.GetType().Name);

        return new AttachmentStorageException(
            operation,
            storageException?.ErrorCode,
            storageException?.Status,
            exception);
    }

    private Task<Azure.Response<BlobContainerInfo>?> EnsureContainerAsync(
        CancellationToken cancellationToken)
    {
        return _container.CreateIfNotExistsAsync(
            publicAccessType: PublicAccessType.None,
            cancellationToken: cancellationToken);
    }
}
