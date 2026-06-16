namespace Api.Features.Authentication.RegisterCompany;

public sealed record RegisterCompanyResponse(
    Guid CompanyId,
    Guid UserId);
