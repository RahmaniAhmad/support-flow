using Api.Errors.ErrorCodes;
using Api.Errors.ErrorMessages;
using Api.Exceptions;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Authentication;
using Shared.Domain;
using Shared.Domain.Users;

namespace Api.Features.Users.CreateUser;

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
               UserErrorMessages.EmailAlreadyExists,
               UserErrorCodes.EmailAlreadyExists);
        }


        Guid? companyId = null;


        if (_currentUser.Role != UserRole.SuperAdmin)
        {
            companyId = _currentUser.CompanyId
            ?? throw new ForbiddenException(
                CommonErrorMessages.CompanyContextRequired,
                CommonErrorCodes.CompanyContextRequired);
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
                   UserErrorMessages.CannotCreateSuperAdmin,
                   UserErrorCodes.CannotCreateSuperAdmin);
            }

            return;
        }


        if (_currentUser.Role == UserRole.Admin)
        {
            if (role != UserRole.Agent &&
                role != UserRole.Customer)
            {
                throw new ForbiddenException(
                 UserErrorMessages.AdminCannotCreateThisRole,
                 UserErrorCodes.AdminCannotCreateThisRole);
            }

            return;
        }


        throw new ForbiddenException(
            UserErrorMessages.CannotCreateUsers,
            UserErrorCodes.CannotCreateUsers);
    }
}