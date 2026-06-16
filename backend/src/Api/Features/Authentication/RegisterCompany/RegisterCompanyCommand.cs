using MediatR;

namespace Api.Features.Authentication.RegisterCompany;

public sealed record RegisterCompanyCommand(
    string CompanyName,
    string Email,
    string Password) : IRequest<RegisterCompanyResponse>;
