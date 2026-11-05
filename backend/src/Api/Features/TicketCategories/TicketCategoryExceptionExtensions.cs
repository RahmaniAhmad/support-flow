using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Api.Features.TicketCategories;

internal static class TicketCategoryExceptionExtensions
{
    public static bool IsDuplicateNameViolation(
        this DbUpdateException exception)
    {
        return exception.InnerException is PostgresException
        {
            SqlState: PostgresErrorCodes.UniqueViolation,
            ConstraintName: "IX_TicketCategories_CompanyId_NormalizedName"
        };
    }
}