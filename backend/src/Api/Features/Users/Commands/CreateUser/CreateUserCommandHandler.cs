using Api.Errors;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Domain.Users;

namespace Api.Features.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly SupportFlowDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ICurrentUser _currentUser;


    public CreateUserCommandHandler(
        SupportFlowDbContext db,
        IPasswordHasher passwordHasher,
        ICurrentUser currentUser)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _currentUser = currentUser;
    }


    public async Task<CreateUserResponse> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        ValidateRoleCreation(request.Role);


        var exists = await _db.Users
            .AnyAsync(
                x => x.Email == request.Email,
                cancellationToken);


        if (exists)
        {
            throw new ConflictException(
               UserErrors.EmailAlreadyExistsMessage,
               UserErrors.EmailAlreadyExistsCode);
        }


        Guid? companyId = null;


        if (_currentUser.Role != UserRole.SuperAdmin)
        {
            companyId = _currentUser.CompanyId
            ?? throw new ForbiddenException(
                CommonErrors.CompanyContextRequiredMessage,
                CommonErrors.CompanyContextRequiredCode);
        }


        var passwordHash =
            _passwordHasher.Hash(request.Password);


        var user = User.Create(
            companyId,
            request.Email,
            passwordHash,
            request.Role);


        user.UpdateProfile(
            request.FirstName,
            request.LastName,
            request.Phone);


        _db.Users.Add(user);


        await _db.SaveChangesAsync(
            cancellationToken);


        return new CreateUserResponse(user.Id);
    }


    private void ValidateRoleCreation(
        UserRole role)
    {
        if (_currentUser.Role == UserRole.SuperAdmin)
        {
            if (role == UserRole.SuperAdmin)
            {
                throw new ForbiddenException(
                   UserErrors.CannotCreateSuperAdminMessage,
                   UserErrors.CannotCreateSuperAdminCode);
            }

            return;
        }


        if (_currentUser.Role == UserRole.Admin)
        {
            if (role != UserRole.Agent &&
                role != UserRole.Customer)
            {
                throw new ForbiddenException(
                 UserErrors.AdminCannotCreateThisRoleMessage,
                 UserErrors.AdminCannotCreateThisRoleCode);
            }

            return;
        }


        throw new ForbiddenException(
            UserErrors.CannotCreateUsersMessage,
            UserErrors.CannotCreateUsersCode);
    }
}