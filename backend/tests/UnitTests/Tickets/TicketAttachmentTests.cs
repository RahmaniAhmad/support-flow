using System.Security.Claims;
using Api.Authorization;
using Api.Exceptions;
using Api.Features.Tickets.Attachments;
using Api.Features.Tickets.Attachments.DeleteTicketAttachment;
using Api.Features.Tickets.Attachments.DownloadTicketAttachment;
using Api.Features.Tickets.Attachments.GetTicketAttachments;
using Api.Features.Tickets.Attachments.UploadTicketAttachment;
using Api.Features.Tickets.Commands.CreateTicket;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Shared.Authentication;
using Shared.Contracts.Storage;
using Shared.Domain.AI;
using Shared.Domain.Base;
using Shared.Domain.Tickets;
using Shared.Domain.Users;

namespace UnitTests.Tickets;

public sealed class TicketAttachmentTests
{
    [Fact]
    public async Task Upload_saves_metadata_and_blob()
    {
        await using var fixture = await Fixture.CreateAsync(UserRole.Customer);
        var storage = new TestAttachmentStorage();
        var handler = fixture.CreateUploadHandler(storage);

        var result = await handler.Handle(
            fixture.UploadRequest(PdfFile()),
            CancellationToken.None);

        Assert.Equal("report.pdf", result.FileName);
        Assert.Equal("application/pdf", result.ContentType);
        Assert.Equal(1, await fixture.Db.Attachments.CountAsync());
        Assert.Single(storage.Files);
    }

    [Fact]
    public async Task Upload_rejects_invalid_file_signature_without_storage_write()
    {
        await using var fixture = await Fixture.CreateAsync(UserRole.Customer);
        var storage = new TestAttachmentStorage();
        var handler = fixture.CreateUploadHandler(storage);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(
            () => handler.Handle(
                fixture.UploadRequest(
                    new TestFile("report.pdf", "application/pdf", [0, 1, 2])),
                CancellationToken.None));

        Assert.Empty(storage.Files);
        Assert.Empty(fixture.Db.Attachments);
    }

    [Fact]
    public async Task Upload_rejects_unapproved_extensions_and_mime_types()
    {
        await using var fixture = await Fixture.CreateAsync(UserRole.Customer);
        var storage = new TestAttachmentStorage();
        var handler = fixture.CreateUploadHandler(storage);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(
            () => handler.Handle(
                fixture.UploadRequest(
                    new TestFile("payload.exe", "application/pdf", "%PDF-"u8.ToArray())),
                CancellationToken.None));

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(
            () => handler.Handle(
                fixture.UploadRequest(
                    new TestFile("payload.pdf", "text/plain", "%PDF-"u8.ToArray())),
                CancellationToken.None));

        Assert.Empty(storage.Files);
    }

    [Fact]
    public async Task Upload_rejects_files_over_10_megabytes()
    {
        await using var fixture = await Fixture.CreateAsync(UserRole.Customer);
        var storage = new TestAttachmentStorage();
        var bytes = new byte[TicketAttachmentValidator.MaximumFileSize + 1];
        "%PDF-"u8.CopyTo(bytes);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(
            () => fixture.CreateUploadHandler(storage).Handle(
                fixture.UploadRequest(
                    new TestFile("report.pdf", "application/pdf", bytes)),
                CancellationToken.None));

        Assert.Empty(storage.Files);
    }

    [Fact]
    public async Task Upload_rejects_sixth_attachment()
    {
        await using var fixture = await Fixture.CreateAsync(UserRole.Customer);
        for (var index = 0; index < TicketAttachmentValidator.MaximumAttachmentsPerTicket; index++)
        {
            fixture.Db.Attachments.Add(
                Attachment.Create(
                    fixture.CompanyId,
                    fixture.TicketId,
                    fixture.UserId,
                    $"file-{index}.pdf",
                    $"{Guid.NewGuid():N}.pdf",
                    "application/pdf",
                    5,
                    $"{fixture.CompanyId:N}/{fixture.TicketId:N}/{Guid.NewGuid():N}.pdf"));
        }

        await fixture.Db.SaveChangesAsync();
        var storage = new TestAttachmentStorage();

        await Assert.ThrowsAsync<ConflictException>(
            () => fixture.CreateUploadHandler(storage).Handle(
                fixture.UploadRequest(PdfFile()),
                CancellationToken.None));

        Assert.Empty(storage.Files);
    }

    [Fact]
    public async Task Customer_cannot_upload_to_another_company_ticket()
    {
        await using var fixture = await Fixture.CreateAsync(
            UserRole.Customer,
            ticketCompanyId: Guid.NewGuid());
        var storage = new TestAttachmentStorage();

        await Assert.ThrowsAsync<NotFoundException>(
            () => fixture.CreateUploadHandler(storage).Handle(
                fixture.UploadRequest(PdfFile()),
                CancellationToken.None));

        Assert.Empty(storage.Files);
    }

    [Fact]
    public async Task Blob_failure_does_not_save_metadata()
    {
        await using var fixture = await Fixture.CreateAsync(UserRole.Customer);
        var storage = new TestAttachmentStorage { FailUpload = true };

        await Assert.ThrowsAsync<IOException>(
            () => fixture.CreateUploadHandler(storage).Handle(
                fixture.UploadRequest(PdfFile()),
                CancellationToken.None));

        Assert.Empty(fixture.Db.Attachments);
    }

    [Fact]
    public async Task Database_failure_after_upload_cleans_up_blob()
    {
        await using var fixture = await Fixture.CreateAsync(UserRole.Customer);
        var storage = new TestAttachmentStorage();
        fixture.TestDb.FailNextSave = true;

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => fixture.CreateUploadHandler(storage).Handle(
                fixture.UploadRequest(PdfFile()),
                CancellationToken.None));

        Assert.Empty(storage.Files);
        Assert.Empty(await fixture.Db.Attachments.ToListAsync());
    }

    [Fact]
    public async Task Create_ticket_without_attachments_preserves_existing_flow()
    {
        await using var fixture = await Fixture.CreateAsync(UserRole.Customer);
        var storage = new TestAttachmentStorage();
        var handler = fixture.CreateCreateTicketHandler(storage);

        var ticketId = await handler.Handle(
            new CreateTicketCommand("New ticket", "Ticket description"),
            CancellationToken.None);

        Assert.NotEqual(Guid.Empty, ticketId);
        Assert.Equal(2, await fixture.Db.Tickets.CountAsync());
        Assert.Empty(await fixture.Db.Attachments.ToListAsync());
        Assert.Empty(storage.Files);
    }

    [Fact]
    public async Task Create_ticket_with_multiple_attachments_saves_ticket_and_metadata()
    {
        await using var fixture = await Fixture.CreateAsync(UserRole.Customer);
        var storage = new TestAttachmentStorage();
        var handler = fixture.CreateCreateTicketHandler(storage);
        var files = new[]
        {
            fixture.FormFile(PdfFile(), "attachments"),
            fixture.FormFile(PdfFile() with { FileName = "second.pdf" }, "attachments")
        };

        var ticketId = await handler.Handle(
            new CreateTicketCommand(
                "New ticket",
                "Ticket description",
                Attachments: files),
            CancellationToken.None);

        var createdAttachments = await fixture.Db.Attachments
            .Where(x => x.TicketId == ticketId)
            .ToListAsync();
        Assert.Equal(2, createdAttachments.Count);
        Assert.Equal(2, storage.Files.Count);
        Assert.All(createdAttachments, item =>
        {
            Assert.Equal(ticketId, item.TicketId);
            Assert.Equal(fixture.CompanyId, item.CompanyId);
            Assert.Equal(fixture.UserId, item.UploadedByUserId);
        });
    }

    [Fact]
    public async Task Create_ticket_with_one_attachment_saves_metadata()
    {
        await using var fixture = await Fixture.CreateAsync(UserRole.Customer);
        var storage = new TestAttachmentStorage();

        var ticketId = await fixture.CreateCreateTicketHandler(storage).Handle(
            new CreateTicketCommand(
                "New ticket",
                "Ticket description",
                Attachments:
                [
                    fixture.FormFile(PdfFile(), "attachments")
                ]),
            CancellationToken.None);

        var attachment = await fixture.Db.Attachments
            .SingleAsync(x => x.TicketId == ticketId);
        Assert.Equal("report.pdf", attachment.FileName);
        Assert.Single(storage.Files);
    }

    [Fact]
    public async Task Create_ticket_storage_failure_cleans_uploaded_blobs_and_does_not_save_ticket()
    {
        await using var fixture = await Fixture.CreateAsync(UserRole.Customer);
        var storage = new TestAttachmentStorage { FailUploadNumber = 2 };
        var handler = fixture.CreateCreateTicketHandler(storage);
        var files = new[]
        {
            fixture.FormFile(PdfFile(), "attachments"),
            fixture.FormFile(PdfFile() with { FileName = "second.pdf" }, "attachments")
        };

        await Assert.ThrowsAsync<IOException>(
            () => handler.Handle(
                new CreateTicketCommand(
                    "New ticket",
                    "Ticket description",
                    Attachments: files),
                CancellationToken.None));

        Assert.Equal(1, await fixture.Db.Tickets.CountAsync());
        Assert.Empty(await fixture.Db.Attachments.ToListAsync());
        Assert.Empty(storage.Files);
    }

    [Fact]
    public async Task Create_ticket_database_failure_cleans_uploaded_blobs()
    {
        await using var fixture = await Fixture.CreateAsync(UserRole.Customer);
        var storage = new TestAttachmentStorage();
        fixture.TestDb.FailNextSave = true;
        var handler = fixture.CreateCreateTicketHandler(storage);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.Handle(
                new CreateTicketCommand(
                    "New ticket",
                    "Ticket description",
                    Attachments: [fixture.FormFile(PdfFile(), "attachments")]),
                CancellationToken.None));

        Assert.Equal(1, await fixture.Db.Tickets.CountAsync());
        Assert.Empty(await fixture.Db.Attachments.ToListAsync());
        Assert.Empty(storage.Files);
    }

    [Fact]
    public async Task Create_ticket_rejects_more_than_five_attachments_before_storage_write()
    {
        await using var fixture = await Fixture.CreateAsync(UserRole.Customer);
        var storage = new TestAttachmentStorage();
        var files = Enumerable.Range(0, 6)
            .Select(index => fixture.FormFile(
                PdfFile() with { FileName = $"file-{index}.pdf" },
                "attachments"))
            .ToArray();

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(
            () => fixture.CreateCreateTicketHandler(storage).Handle(
                new CreateTicketCommand(
                    "New ticket",
                    "Ticket description",
                    Attachments: files),
                CancellationToken.None));

        Assert.Equal(1, await fixture.Db.Tickets.CountAsync());
        Assert.Empty(storage.Files);
    }

    [Fact]
    public async Task Attachment_list_and_download_require_ticket_access()
    {
        await using var fixture = await Fixture.CreateAsync(UserRole.Customer);
        var storage = new TestAttachmentStorage();
        var uploaded = await fixture.CreateUploadHandler(storage).Handle(
            fixture.UploadRequest(PdfFile()),
            CancellationToken.None);
        var listHandler = new GetTicketAttachmentsQueryHandler(
            fixture.Db,
            fixture.Access);
        var downloadHandler = new DownloadTicketAttachmentQueryHandler(
            fixture.Db,
            fixture.Access,
            storage);

        var list = await listHandler.Handle(
            new GetTicketAttachmentsQuery(fixture.TicketId),
            CancellationToken.None);
        var download = await downloadHandler.Handle(
            new DownloadTicketAttachmentQuery(uploaded.Id),
            CancellationToken.None);

        Assert.Single(list!);
        Assert.Equal("report.pdf", download!.FileName);
        using var content = new MemoryStream();
        await download.Content.CopyToAsync(content);
        Assert.Equal(PdfFile().Bytes, content.ToArray());
    }

    [Fact]
    public async Task Uploader_can_delete_and_blob_failure_prevents_success()
    {
        await using var fixture = await Fixture.CreateAsync(UserRole.Customer);
        var storage = new TestAttachmentStorage();
        var uploaded = await fixture.CreateUploadHandler(storage).Handle(
            fixture.UploadRequest(PdfFile()),
            CancellationToken.None);
        var accessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(
                    [new Claim(ClaimTypes.NameIdentifier, fixture.UserId.ToString())],
                    "test"))
            }
        };
        var handler = new DeleteTicketAttachmentCommandHandler(
            fixture.Db,
            fixture.CurrentUser,
            fixture.Access,
            storage,
            new FailedAuthorizationService(),
            accessor,
            NullLogger<DeleteTicketAttachmentCommandHandler>.Instance);

        storage.FailDelete = true;
        await Assert.ThrowsAsync<IOException>(
            () => handler.Handle(
                new DeleteTicketAttachmentCommand(uploaded.Id),
                CancellationToken.None));
        Assert.Single(await fixture.Db.Attachments.ToListAsync());

        storage.FailDelete = false;
        await handler.Handle(
            new DeleteTicketAttachmentCommand(uploaded.Id),
            CancellationToken.None);
        Assert.Empty(await fixture.Db.Attachments.ToListAsync());
        Assert.Empty(storage.Files);
    }

    [Fact]
    public async Task Database_failure_after_blob_delete_restores_blob()
    {
        await using var fixture = await Fixture.CreateAsync(UserRole.Customer);
        var storage = new TestAttachmentStorage();
        var uploaded = await fixture.CreateUploadHandler(storage).Handle(
            fixture.UploadRequest(PdfFile()),
            CancellationToken.None);
        var accessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(
                    [new Claim(ClaimTypes.NameIdentifier, fixture.UserId.ToString())],
                    "test"))
            }
        };
        var handler = fixture.CreateDeleteHandler(storage, accessor);
        fixture.TestDb.FailNextSave = true;

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.Handle(
                new DeleteTicketAttachmentCommand(uploaded.Id),
                CancellationToken.None));

        Assert.Single(storage.Files);
        Assert.Single(await fixture.Db.Attachments.ToListAsync());
    }

    [Fact]
    public void Attachment_management_permission_is_limited_to_administrators()
    {
        Assert.Contains(
            Permissions.TicketsManageAttachments,
            RolePermissions.Map[UserRole.Admin]);
        Assert.Contains(
            Permissions.TicketsManageAttachments,
            RolePermissions.Map[UserRole.SuperAdmin]);
        Assert.DoesNotContain(
            Permissions.TicketsManageAttachments,
            RolePermissions.Map[UserRole.Agent]);
        Assert.DoesNotContain(
            Permissions.TicketsManageAttachments,
            RolePermissions.Map[UserRole.Customer]);
    }

    private static TestFile PdfFile() =>
        new("report.pdf", "application/pdf", "%PDF-1.7 test"u8.ToArray());

    private sealed record TestFile(
        string FileName,
        string ContentType,
        byte[] Bytes);

    private sealed class TestAttachmentStorage : IAttachmentStorage
    {
        public Dictionary<string, byte[]> Files { get; } = [];
        public bool FailUpload { get; set; }
        public bool FailDelete { get; set; }
        public int? FailUploadNumber { get; set; }
        private int _uploadCount;

        public async Task UploadAsync(
            string storagePath,
            Stream content,
            string contentType,
            CancellationToken cancellationToken)
        {
            _uploadCount++;
            if (FailUpload || _uploadCount == FailUploadNumber)
            {
                throw new IOException("Storage unavailable.");
            }

            using var buffer = new MemoryStream();
            await content.CopyToAsync(buffer, cancellationToken);
            Files[storagePath] = buffer.ToArray();
        }

        public Task<Stream> OpenReadAsync(
            string storagePath,
            CancellationToken cancellationToken)
        {
            if (!Files.TryGetValue(storagePath, out var content))
            {
                throw new FileNotFoundException();
            }

            return Task.FromResult<Stream>(
                new MemoryStream(content, writable: false));
        }

        public Task DeleteAsync(
            string storagePath,
            CancellationToken cancellationToken)
        {
            if (FailDelete)
            {
                throw new IOException("Storage unavailable.");
            }

            Files.Remove(storagePath);
            return Task.CompletedTask;
        }
    }

    private sealed class Fixture : IAsyncDisposable
    {
        private Fixture(
            SupportFlowDbContext db,
            Guid companyId,
            Guid ticketId,
            Guid userId,
            UserRole role)
        {
            Db = db;
            CompanyId = companyId;
            TicketId = ticketId;
            UserId = userId;
            CurrentUser = new TestCurrentUser(userId, companyId, role);
            Access = new TicketAccessService(CurrentUser);
        }

        public SupportFlowDbContext Db { get; }
        public TestSupportFlowDbContext TestDb => (TestSupportFlowDbContext)Db;
        public Guid CompanyId { get; }
        public Guid TicketId { get; }
        public Guid UserId { get; }
        public TestCurrentUser CurrentUser { get; }
        public TicketAccessService Access { get; }

        public static async Task<Fixture> CreateAsync(
            UserRole role,
            Guid? ticketCompanyId = null)
        {
            var companyId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var db = CreateDbContext();
            var ticket = Ticket.Create(
                ticketCompanyId ?? companyId,
                userId,
                "Subject",
                "Description");
            var ticketId = ticket.Id;
            db.Tickets.Add(ticket);
            await db.SaveChangesAsync();

            return new Fixture(db, companyId, ticketId, userId, role);
        }

        public UploadTicketAttachmentCommand UploadRequest(TestFile file)
        {
            return new UploadTicketAttachmentCommand(
                TicketId,
                FormFile(file, "file"));
        }

        public FormFile FormFile(TestFile file, string fieldName)
        {
            var stream = new MemoryStream(file.Bytes);
            return new FormFile(
                stream,
                0,
                file.Bytes.LongLength,
                fieldName,
                file.FileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = file.ContentType
            };
        }

        public UploadTicketAttachmentCommandHandler CreateUploadHandler(
            IAttachmentStorage storage)
        {
            var processor = CreateAttachmentProcessor(storage);
            return new(
                Db,
                CurrentUser,
                Access,
                processor,
                NullLogger<UploadTicketAttachmentCommandHandler>.Instance);
        }

        public CreateTicketCommandHandler CreateCreateTicketHandler(
            IAttachmentStorage storage) =>
            new(Db, CurrentUser, CreateAttachmentProcessor(storage));

        private TicketAttachmentProcessor CreateAttachmentProcessor(
            IAttachmentStorage storage) =>
            new(
                Db,
                storage,
                NullLogger<TicketAttachmentProcessor>.Instance);

        public DeleteTicketAttachmentCommandHandler CreateDeleteHandler(
            IAttachmentStorage storage,
            IHttpContextAccessor accessor) =>
            new(
                Db,
                CurrentUser,
                Access,
                storage,
                new FailedAuthorizationService(),
                accessor,
                NullLogger<DeleteTicketAttachmentCommandHandler>.Instance);

        public ValueTask DisposeAsync() => Db.DisposeAsync();

        private static SupportFlowDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<SupportFlowDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TestSupportFlowDbContext(
                options,
                new TestDomainEventDispatcher());
        }
    }

    private sealed class TestSupportFlowDbContext(
        DbContextOptions<SupportFlowDbContext> options,
        IDomainEventDispatcher dispatcher)
        : SupportFlowDbContext(options, dispatcher)
    {
        public bool FailNextSave { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EmbeddingDocument>().Ignore(x => x.Vector);
        }

        public override Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            if (FailNextSave)
            {
                FailNextSave = false;
                throw new InvalidOperationException("Simulated database failure.");
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }

    private sealed record TestCurrentUser(
        Guid UserId,
        Guid? CompanyId,
        UserRole Role) : ICurrentUser
    {
        public string Email => "test@example.com";
    }

    private sealed class TestDomainEventDispatcher : IDomainEventDispatcher
    {
        public Task DispatchAsync(
            IEnumerable<IDomainEvent> domainEvents,
            CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }

    private sealed class FailedAuthorizationService : IAuthorizationService
    {
        public Task<AuthorizationResult> AuthorizeAsync(
            ClaimsPrincipal user,
            object? resource,
            IEnumerable<IAuthorizationRequirement> requirements) =>
            Task.FromResult(AuthorizationResult.Failed());

        public Task<AuthorizationResult> AuthorizeAsync(
            ClaimsPrincipal user,
            object? resource,
            string policyName) =>
            Task.FromResult(AuthorizationResult.Failed());
    }
}
