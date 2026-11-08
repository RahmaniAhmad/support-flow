using Api.Authorization;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Contracts.Storage;
using Shared.Domain.Users;

namespace Api.Features.Tickets.Attachments.DeleteTicketAttachment;

public sealed class DeleteTicketAttachmentCommandHandler
    : IRequestHandler<DeleteTicketAttachmentCommand>
{
    private readonly SupportFlowDbContext _db;
    private readonly ICurrentUser _currentUser;
    private readonly ITicketAccessService _ticketAccess;
    private readonly IAttachmentStorage _storage;
    private readonly IAuthorizationService _authorization;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<DeleteTicketAttachmentCommandHandler> _logger;

    public DeleteTicketAttachmentCommandHandler(
        SupportFlowDbContext db,
        ICurrentUser currentUser,
        ITicketAccessService ticketAccess,
        IAttachmentStorage storage,
        IAuthorizationService authorization,
        IHttpContextAccessor httpContextAccessor,
        ILogger<DeleteTicketAttachmentCommandHandler> logger)
    {
        _db = db;
        _currentUser = currentUser;
        _ticketAccess = ticketAccess;
        _storage = storage;
        _authorization = authorization;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task Handle(
        DeleteTicketAttachmentCommand request,
        CancellationToken cancellationToken)
    {
        var accessibleTickets = _ticketAccess
            .ApplyTicketAccessFilter(_db.Tickets.AsNoTracking());

        var attachment = await _db.Attachments
            .AsNoTracking()
            .Where(x =>
                x.Id == request.Id &&
                accessibleTickets
                    .Any(ticket =>
                        ticket.Id == x.TicketId &&
                        ticket.CompanyId == x.CompanyId))
            .FirstOrDefaultAsync(cancellationToken);

        if (attachment is null)
        {
            throw new NotFoundException("Attachment not found.");
        }

        var isUploader = attachment.UploadedByUserId == _currentUser.UserId;
        var httpContext = _httpContextAccessor.HttpContext;
        var canManageAttachments = httpContext is not null &&
            (await _authorization.AuthorizeAsync(
                httpContext.User,
                attachment,
                new PermissionRequirement(
                    Permissions.TicketsManageAttachments))).Succeeded;

        if (!isUploader && !canManageAttachments)
        {
            throw new ForbiddenException(
                "You are not allowed to delete this attachment.",
                TicketAttachmentErrors.DeleteForbidden);
        }

        await using var backup = new MemoryStream();
        await using (var content = await _storage.OpenReadAsync(
                         attachment.StoragePath,
                         cancellationToken))
        {
            await content.CopyToAsync(backup, cancellationToken);
        }

        backup.Position = 0;
        await _storage.DeleteAsync(
            attachment.StoragePath,
            cancellationToken);

        var trackedAttachment = _db.Attachments.Local
            .FirstOrDefault(x => x.Id == attachment.Id);

        if (trackedAttachment is null)
        {
            _db.Attachments.Attach(attachment);
            trackedAttachment = attachment;
        }

        _db.Attachments.Remove(trackedAttachment);

        try
        {
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            backup.Position = 0;

            try
            {
                await _storage.UploadAsync(
                    attachment.StoragePath,
                    backup,
                    attachment.ContentType,
                    CancellationToken.None);
            }
            catch (Exception restoreException)
            {
                _logger.LogError(
                    restoreException,
                    "Attachment blob restore failed after metadata deletion failed.");
            }

            throw;
        }
    }
}
