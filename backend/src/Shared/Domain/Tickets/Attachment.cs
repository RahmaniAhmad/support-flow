using Shared.Domain.Base;

namespace Shared.Domain.Tickets;

public sealed class Attachment : Entity
{
    public Guid CompanyId { get; private set; }

    public Guid TicketId { get; private set; }

    public Guid UploadedByUserId { get; private set; }

    public string FileName { get; private set; } = string.Empty;

    public string StoredFileName { get; private set; } = string.Empty;

    public string ContentType { get; private set; } = string.Empty;

    public long FileSize { get; private set; }

    public string StoragePath { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }

    private Attachment()
    {
    }

    public static Attachment Create(
        Guid companyId,
        Guid ticketId,
        Guid uploadedByUserId,
        string fileName,
        string storedFileName,
        string contentType,
        long fileSize,
        string storagePath)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(companyId, Guid.Empty);
        ArgumentOutOfRangeException.ThrowIfEqual(ticketId, Guid.Empty);
        ArgumentOutOfRangeException.ThrowIfEqual(uploadedByUserId, Guid.Empty);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(storedFileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(fileSize);
        ArgumentException.ThrowIfNullOrWhiteSpace(storagePath);

        return new Attachment
        {
            CompanyId = companyId,
            TicketId = ticketId,
            UploadedByUserId = uploadedByUserId,
            FileName = fileName,
            StoredFileName = storedFileName,
            ContentType = contentType,
            FileSize = fileSize,
            StoragePath = storagePath,
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}
