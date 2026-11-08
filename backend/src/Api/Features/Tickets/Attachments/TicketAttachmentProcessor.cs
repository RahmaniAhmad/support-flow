using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Storage;
using Shared.Domain.Tickets;

namespace Api.Features.Tickets.Attachments;

public sealed class TicketAttachmentProcessor
{
    private readonly SupportFlowDbContext _db;
    private readonly IAttachmentStorage _storage;
    private readonly ILogger<TicketAttachmentProcessor> _logger;

    public TicketAttachmentProcessor(
        SupportFlowDbContext db,
        IAttachmentStorage storage,
        ILogger<TicketAttachmentProcessor> logger)
    {
        _db = db;
        _storage = storage;
        _logger = logger;
    }

    public async Task<IReadOnlyList<Attachment>> UploadAndStageAsync(
        Ticket ticket,
        Guid uploadedByUserId,
        IReadOnlyList<TicketAttachmentValidator.ValidatedAttachmentFile> files,
        CancellationToken cancellationToken)
    {
        var uploadedPaths = new List<string>(files.Count);
        var attachments = new List<Attachment>(files.Count);

        try
        {
            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                var storedFileName = $"{Guid.NewGuid():N}{extension}";
                var storagePath =
                    $"{ticket.CompanyId:N}/{ticket.Id:N}/{storedFileName}";

                uploadedPaths.Add(storagePath);

                await using var content = file.File.OpenReadStream();
                await _storage.UploadAsync(
                    storagePath,
                    content,
                    file.ContentType,
                    cancellationToken);

                var attachment = Attachment.Create(
                    ticket.CompanyId,
                    ticket.Id,
                    uploadedByUserId,
                    file.FileName,
                    storedFileName,
                    file.ContentType,
                    file.File.Length,
                    storagePath);

                _db.Attachments.Add(attachment);
                attachments.Add(attachment);
            }

            return attachments;
        }
        catch
        {
            await CleanupAsync(uploadedPaths);
            throw;
        }
    }

    public async Task CleanupAsync(IEnumerable<string> storagePaths)
    {
        foreach (var storagePath in storagePaths)
        {
            try
            {
                await _storage.DeleteAsync(
                    storagePath,
                    CancellationToken.None);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    "Attachment blob cleanup failed. Exception type: {ExceptionType}.",
                    exception.GetType().Name);
            }
        }
    }
}
