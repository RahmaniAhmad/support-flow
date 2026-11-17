using Api.Authorization;
using Api.Exceptions;
using Api.Features.Dashboard.Agents;
using Api.Features.Dashboard.Statistics;
using Api.Features.Tickets.Attachments.DownloadTicketAttachment;
using Api.Features.Tickets.Commands.AddComment;
using Api.Features.Tickets.Commands.AssignTicket;
using Api.Features.Tickets.Commands.ChangeTicketPriority;
using Api.Features.Tickets.Queries.GetTicket;
using Api.Features.Tickets.Queries.GetTickets;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Domain.AI;
using Shared.Contracts.Storage;
using Shared.Domain.Base;
using Shared.Domain.Companies;
using Shared.Domain.Tickets;
using Shared.Domain.Users;

namespace UnitTests.Tickets;

public sealed class TicketAuthorizationIsolationTests
{
    [Fact]
    public async Task Agent_can_list_own_and_unassigned_but_not_other_agents_or_tenants()
    {
        await using var db = CreateDbContext();
        var companyA = Company.Create("Company A");
        var companyB = Company.Create("Company B");
        var creator = CreateUser(companyA.Id, UserRole.Customer, "creator");
        var agent1 = CreateUser(companyA.Id, UserRole.Agent, "agent1");
        var agent2 = CreateUser(companyA.Id, UserRole.Agent, "agent2");
        var otherCompanyCreator = CreateUser(companyB.Id, UserRole.Customer, "other");

        var ownTicket = CreateTicket(companyA.Id, creator.Id, "Agent 2 ticket", agent2.Id);
        var otherAgentTicket = CreateTicket(companyA.Id, creator.Id, "Agent 1 ticket", agent1.Id);
        var unassignedTicket = CreateTicket(companyA.Id, creator.Id, "Unassigned ticket");
        var otherCompanyTicket = CreateTicket(companyB.Id, otherCompanyCreator.Id, "Other company", agent2.Id);

        db.Companies.AddRange(companyA, companyB);
        db.Users.AddRange(creator, agent1, agent2, otherCompanyCreator);
        db.Tickets.AddRange(ownTicket, otherAgentTicket, unassignedTicket, otherCompanyTicket);
        await db.SaveChangesAsync();

        var currentUser = new TestCurrentUser(agent2.Id, companyA.Id, UserRole.Agent);
        var handler = new GetTicketsQueryHandler(
            db,
            currentUser,
            new TicketAccessService(currentUser));

        var result = await handler.Handle(new GetTicketsQuery(), CancellationToken.None);

        Assert.Equal(2, result.TotalCount);
        Assert.Contains(result.Items, x => x.Id == ownTicket.Id);
        Assert.Contains(result.Items, x => x.Id == unassignedTicket.Id);
        Assert.DoesNotContain(result.Items, x => x.Id == otherAgentTicket.Id);
        Assert.DoesNotContain(result.Items, x => x.Id == otherCompanyTicket.Id);

        var searchResults = await new TicketAccessService(currentUser)
            .ApplyTicketAccessFilter(db.Tickets.AsNoTracking())
            .Where(x => x.Subject.Contains("Agent 1"))
            .ToListAsync();
        Assert.Empty(searchResults);
    }

    [Fact]
    public async Task Ticket_list_sorts_before_pagination_and_returns_filtered_total()
    {
        await using var db = CreateDbContext();
        var company = Company.Create("Company");
        var creator = CreateUser(company.Id, UserRole.Customer, "creator");
        var tickets = new[]
        {
            CreateTicket(company.Id, creator.Id, "Charlie"),
            CreateTicket(company.Id, creator.Id, "Alpha"),
            CreateTicket(company.Id, creator.Id, "Bravo")
        };
        db.Companies.Add(company);
        db.Users.Add(creator);
        db.Tickets.AddRange(tickets);
        await db.SaveChangesAsync();

        var currentUser = new TestCurrentUser(creator.Id, company.Id, UserRole.Customer);
        var handler = new GetTicketsQueryHandler(
            db,
            currentUser,
            new TicketAccessService(currentUser));

        var firstPage = await handler.Handle(
            new GetTicketsQuery(Page: 1, PageSize: 1, SortBy: "Subject",
                SortDirection: Shared.Contracts.Sorting.SortDirection.Asc),
            CancellationToken.None);
        var secondPage = await handler.Handle(
            new GetTicketsQuery(Page: 2, PageSize: 1, SortBy: "Subject",
                SortDirection: Shared.Contracts.Sorting.SortDirection.Asc),
            CancellationToken.None);
        var largerPage = await handler.Handle(
            new GetTicketsQuery(Page: 1, PageSize: 2, SortBy: "Subject",
                SortDirection: Shared.Contracts.Sorting.SortDirection.Asc),
            CancellationToken.None);

        Assert.Equal(3, firstPage.TotalCount);
        Assert.Equal(3, secondPage.TotalCount);
        Assert.Equal("Alpha", Assert.Single(firstPage.Items).Subject);
        Assert.Equal("Bravo", Assert.Single(secondPage.Items).Subject);
        Assert.Equal(2, largerPage.Items.Count);
        Assert.Equal(2, largerPage.PageSize);
    }

    [Fact]
    public async Task Agent_cannot_retrieve_or_modify_another_agents_ticket_by_id()
    {
        await using var db = CreateDbContext();
        var company = Company.Create("Company");
        var creator = CreateUser(company.Id, UserRole.Customer, "creator");
        var agent1 = CreateUser(company.Id, UserRole.Agent, "agent1");
        var agent2 = CreateUser(company.Id, UserRole.Agent, "agent2");
        var ticket = CreateTicket(company.Id, creator.Id, "Private ticket", agent1.Id);
        db.Companies.Add(company);
        db.Users.AddRange(creator, agent1, agent2);
        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();

        var currentUser = new TestCurrentUser(agent2.Id, company.Id, UserRole.Agent);
        var accessService = new TicketAccessService(currentUser);
        var getHandler = new GetTicketQueryHandler(db, accessService);
        var changePriorityHandler = new ChangeTicketPriorityCommandHandler(
            db,
            currentUser,
            accessService);
        var ownerGetHandler = new GetTicketQueryHandler(
            db,
            new TicketAccessService(
                new TestCurrentUser(agent1.Id, company.Id, UserRole.Agent)));

        Assert.Equal(
            ticket.Id,
            (await ownerGetHandler.Handle(
                new GetTicketQuery(ticket.Id),
                CancellationToken.None))!.Id);
        await Assert.ThrowsAsync<NotFoundException>(() =>
            getHandler.Handle(new GetTicketQuery(ticket.Id), CancellationToken.None));
        await Assert.ThrowsAsync<NotFoundException>(() =>
            changePriorityHandler.Handle(
                new ChangeTicketPriorityCommand(ticket.Id, TicketPriority.High),
                CancellationToken.None));
    }

    [Fact]
    public async Task Agent_cannot_comment_on_another_agents_ticket()
    {
        await using var db = CreateDbContext();
        var company = Company.Create("Company");
        var creator = CreateUser(company.Id, UserRole.Customer, "creator");
        var agent1 = CreateUser(company.Id, UserRole.Agent, "agent1");
        var agent2 = CreateUser(company.Id, UserRole.Agent, "agent2");
        var ticket = CreateTicket(company.Id, creator.Id, "Private ticket", agent1.Id);
        db.Companies.Add(company);
        db.Users.AddRange(creator, agent1, agent2);
        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();

        var currentUser = new TestCurrentUser(agent2.Id, company.Id, UserRole.Agent);
        var handler = new AddCommentCommandHandler(
            db,
            currentUser,
            new TicketAccessService(currentUser));

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(
                new AddCommentCommand(ticket.Id, "Unauthorized comment"),
                CancellationToken.None));

        Assert.Empty(ticket.Comments);
    }

    [Fact]
    public async Task Customer_cannot_modify_another_customers_ticket()
    {
        await using var db = CreateDbContext();
        var company = Company.Create("Company");
        var owner = CreateUser(company.Id, UserRole.Customer, "owner");
        var otherCustomer = CreateUser(company.Id, UserRole.Customer, "other");
        var ticket = CreateTicket(company.Id, owner.Id, "Customer ticket");
        db.Companies.Add(company);
        db.Users.AddRange(owner, otherCustomer);
        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();

        var currentUser = new TestCurrentUser(
            otherCustomer.Id,
            company.Id,
            UserRole.Customer);
        var handler = new ChangeTicketPriorityCommandHandler(
            db,
            currentUser,
            new TicketAccessService(currentUser));

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(
                new ChangeTicketPriorityCommand(ticket.Id, TicketPriority.High),
                CancellationToken.None));
    }

    [Fact]
    public async Task Agent_cannot_download_another_agents_attachment()
    {
        await using var db = CreateDbContext();
        var company = Company.Create("Company");
        var creator = CreateUser(company.Id, UserRole.Customer, "creator");
        var agent1 = CreateUser(company.Id, UserRole.Agent, "agent1");
        var agent2 = CreateUser(company.Id, UserRole.Agent, "agent2");
        var ticket = CreateTicket(company.Id, creator.Id, "Private ticket", agent1.Id);
        var attachment = Attachment.Create(
            company.Id,
            ticket.Id,
            agent1.Id,
            "private.txt",
            "random-file.txt",
            "text/plain",
            1,
            "company/ticket/random-file.txt");
        db.Companies.Add(company);
        db.Users.AddRange(creator, agent1, agent2);
        db.Tickets.Add(ticket);
        db.Attachments.Add(attachment);
        await db.SaveChangesAsync();

        var storage = new RecordingAttachmentStorage();
        var currentUser = new TestCurrentUser(agent2.Id, company.Id, UserRole.Agent);
        var handler = new DownloadTicketAttachmentQueryHandler(
            db,
            new TicketAccessService(currentUser),
            storage);

        var result = await handler.Handle(
            new DownloadTicketAttachmentQuery(attachment.Id),
            CancellationToken.None);

        Assert.Null(result);
        Assert.Equal(0, storage.ReadCount);
    }

    [Fact]
    public async Task Agent_can_claim_an_unassigned_ticket_for_themselves()
    {
        await using var db = CreateDbContext();
        var company = Company.Create("Company");
        var creator = CreateUser(company.Id, UserRole.Customer, "creator");
        var agent = CreateUser(company.Id, UserRole.Agent, "agent");
        var ticket = CreateTicket(company.Id, creator.Id, "Available ticket");
        db.Companies.Add(company);
        db.Users.AddRange(creator, agent);
        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();

        var currentUser = new TestCurrentUser(agent.Id, company.Id, UserRole.Agent);
        var handler = new AssignTicketCommandHandler(
            db,
            currentUser,
            new TicketAccessService(currentUser));

        await handler.Handle(
            new AssignTicketCommand(ticket.Id, agent.Id),
            CancellationToken.None);

        Assert.Equal(agent.Id, ticket.AssignedToUserId);
    }

    [Fact]
    public async Task Ticket_assignment_rejects_a_user_from_another_company()
    {
        await using var db = CreateDbContext();
        var companyA = Company.Create("Company A");
        var companyB = Company.Create("Company B");
        var creator = CreateUser(companyA.Id, UserRole.Customer, "creator");
        var agent = CreateUser(companyB.Id, UserRole.Agent, "agent");
        var ticket = CreateTicket(companyA.Id, creator.Id, "Company A ticket");
        db.Companies.AddRange(companyA, companyB);
        db.Users.AddRange(creator, agent);
        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();

        var currentUser = new TestCurrentUser(
            Guid.NewGuid(),
            companyA.Id,
            UserRole.Admin);
        var handler = new AssignTicketCommandHandler(
            db,
            currentUser,
            new TicketAccessService(currentUser));

        await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(
                new AssignTicketCommand(ticket.Id, agent.Id),
                CancellationToken.None));
        Assert.Null(ticket.AssignedToUserId);
    }

    [Fact]
    public async Task Dashboard_statistics_only_count_tickets_visible_to_the_agent()
    {
        await using var db = CreateDbContext();
        var companyA = Company.Create("Company A");
        var companyB = Company.Create("Company B");
        var creator = CreateUser(companyA.Id, UserRole.Customer, "creator");
        var agent1 = CreateUser(companyA.Id, UserRole.Agent, "agent1");
        var agent2 = CreateUser(companyA.Id, UserRole.Agent, "agent2");
        var otherCompanyCreator = CreateUser(companyB.Id, UserRole.Customer, "other");
        db.Companies.AddRange(companyA, companyB);
        db.Users.AddRange(creator, agent1, agent2, otherCompanyCreator);
        db.Tickets.AddRange(
            CreateTicket(companyA.Id, creator.Id, "Mine", agent2.Id),
            CreateTicket(companyA.Id, creator.Id, "Other agent", agent1.Id),
            CreateTicket(companyA.Id, creator.Id, "Unassigned"),
            CreateTicket(companyB.Id, otherCompanyCreator.Id, "Other company", agent2.Id));
        await db.SaveChangesAsync();

        var currentUser = new TestCurrentUser(agent2.Id, companyA.Id, UserRole.Agent);
        var handler = new GetDashboardStatisticsQueryHandler(
            db,
            new TicketAccessService(currentUser));

        var result = await handler.Handle(
            new GetDashboardStatisticsQuery(companyA.Id),
            CancellationToken.None);

        Assert.Equal(2, result.TotalTickets);
        Assert.Equal(1, result.AssignedTickets);
        Assert.Equal(1, result.UnassignedTickets);

        var performanceHandler = new GetAgentPerformanceQueryHandler(
            db,
            new TicketAccessService(currentUser));
        var performance = await performanceHandler.Handle(
            new GetAgentPerformanceQuery(companyA.Id),
            CancellationToken.None);

        var visibleAgent = Assert.Single(performance);
        Assert.Equal(agent2.Id, visibleAgent.UserId);
        Assert.Equal(1, visibleAgent.AssignedTickets);
    }

    [Fact]
    public void Admin_and_customer_access_filters_enforce_company_and_creator_boundaries()
    {
        var companyA = Guid.NewGuid();
        var companyB = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var tickets = new[]
        {
            Ticket.Create(companyA, customerId, "Customer ticket", "Description"),
            Ticket.Create(companyA, Guid.NewGuid(), "Same company", "Description"),
            Ticket.Create(companyB, customerId, "Other company", "Description")
        };

        var admin = new TicketAccessService(
            new TestCurrentUser(Guid.NewGuid(), companyA, UserRole.Admin));
        var customer = new TicketAccessService(
            new TestCurrentUser(customerId, companyA, UserRole.Customer));
        var superAdmin = new TicketAccessService(
            new TestCurrentUser(Guid.NewGuid(), null, UserRole.SuperAdmin));

        Assert.Equal(
            2,
            admin.ApplyTicketAccessFilter(tickets.AsQueryable()).Count());
        Assert.Equal(
            tickets[0].Id,
            Assert.Single(customer.ApplyTicketAccessFilter(tickets.AsQueryable())).Id);
        Assert.Equal(
            tickets.Length,
            superAdmin.ApplyTicketAccessFilter(tickets.AsQueryable()).Count());
    }

    private static SupportFlowDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<SupportFlowDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new TestSupportFlowDbContext(options, new NoOpDomainEventDispatcher());
    }

    private static User CreateUser(Guid companyId, UserRole role, string label)
    {
        var user = User.Create(
            companyId,
            $"{label}@example.com",
            "hashed-password",
            role);
        user.UpdateProfile(label, "User", null);
        return user;
    }

    private static Ticket CreateTicket(
        Guid companyId,
        Guid creatorId,
        string subject,
        Guid? assignedToUserId = null)
    {
        var ticket = Ticket.Create(companyId, creatorId, subject, "Ticket description");
        if (assignedToUserId.HasValue)
        {
            ticket.AssignTo(assignedToUserId.Value, assignedToUserId.Value);
        }

        return ticket;
    }

    private sealed record TestCurrentUser(
        Guid UserId,
        Guid? CompanyId,
        UserRole Role) : ICurrentUser
    {
        public string Email => "test@example.com";
    }

    private sealed class NoOpDomainEventDispatcher : IDomainEventDispatcher
    {
        public Task DispatchAsync(
            IEnumerable<IDomainEvent> domainEvents,
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class TestSupportFlowDbContext : SupportFlowDbContext
    {
        public TestSupportFlowDbContext(
            DbContextOptions<SupportFlowDbContext> options,
            IDomainEventDispatcher dispatcher)
            : base(options, dispatcher)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(SupportFlowDbContext).Assembly,
                type => type != typeof(EmbeddingDocumentConfiguration));
            modelBuilder.Ignore<EmbeddingDocument>();
        }
    }

    private sealed class RecordingAttachmentStorage : IAttachmentStorage
    {
        public int ReadCount { get; private set; }

        public Task UploadAsync(
            string storagePath,
            Stream content,
            string contentType,
            CancellationToken cancellationToken) =>
            throw new NotSupportedException();

        public Task<Stream> OpenReadAsync(
            string storagePath,
            CancellationToken cancellationToken)
        {
            ReadCount++;
            return Task.FromResult<Stream>(new MemoryStream());
        }

        public Task DeleteAsync(
            string storagePath,
            CancellationToken cancellationToken) =>
            throw new NotSupportedException();
    }
}
