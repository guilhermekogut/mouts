using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Repositories;

using AutoMapper;

using FluentValidation;

using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.UpdateUser;

/// <summary>
/// Handler for processing UpdateUserCommand requests
/// </summary>
public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UpdateUserResult>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Initializes a new instance of UpdateUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for UpdateUserCommand</param>
    public UpdateUserHandler(IUserRepository userRepository, IMapper mapper, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Handles the UpdateUserCommand request
    /// </summary>
    /// <param name="command">The UpdateUser command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated user details</returns>
    public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateUserCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingUser = await _userRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingUser is null)
            throw new InvalidOperationException($"User with Id {command.Id} not found");

        existingUser.Password = _passwordHasher.HashPassword(command.Password);
        existingUser.Status = command.Status;
        existingUser.Role = command.Role;
        existingUser.Email = command.Email;
        existingUser.Name = new Domain.ValueObjects.Name(command.Name.Firstname, command.Name.Lastname);
        existingUser.Phone = command.Phone;
        existingUser.Address = new Domain.ValueObjects.Address(command.Address.City, command.Address.Street, command.Address.Number, command.Address.Zipcode,
            new Domain.ValueObjects.Geolocation(command.Address.Geolocation.Lat, command.Address.Geolocation.Long));
        existingUser.UpdatedAt = DateTime.UtcNow;
        var updatedUser = await _userRepository.UpdateAsync(existingUser, cancellationToken);
        var result = _mapper.Map<UpdateUserResult>(updatedUser);
        return result;
    }
}
