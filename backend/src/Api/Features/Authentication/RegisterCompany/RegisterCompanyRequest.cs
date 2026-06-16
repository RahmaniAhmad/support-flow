namespace Api.Features.Authentication.RegisterCompany;

public sealed record RegisterCompanyRequest(
    string CompanyName,
    string Email,
    string Password);
