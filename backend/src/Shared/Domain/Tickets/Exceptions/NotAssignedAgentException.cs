using Shared.Domain.Exceptions;

namespace Shared.Domain.Tickets.Exceptions;

public sealed class NotAssignedAgentException()
    : DomainException(
        "Only the assigned agent can perform this action.");