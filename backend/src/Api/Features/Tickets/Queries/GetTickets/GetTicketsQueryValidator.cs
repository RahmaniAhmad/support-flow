using FluentValidation;

namespace Api.Features.Tickets.Queries.GetTickets;

public sealed class GetTicketsQueryValidator : AbstractValidator<GetTicketsQuery>
{
    public GetTicketsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.SortBy)
            .Must(sortBy =>
                sortBy is null ||
                sortBy.Equals("Subject", StringComparison.OrdinalIgnoreCase) ||
                sortBy.Equals("Status", StringComparison.OrdinalIgnoreCase) ||
                sortBy.Equals("CreatedAtUtc", StringComparison.OrdinalIgnoreCase));
    }
}
