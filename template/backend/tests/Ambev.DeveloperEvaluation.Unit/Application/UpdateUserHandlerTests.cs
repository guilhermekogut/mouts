using Ambev.DeveloperEvaluation.Application.Users.Common;
using Ambev.DeveloperEvaluation.Application.Users.UpdateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Unit.Domain;

using AutoMapper;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="UpdateUserHandler"/> class.
/// </summary>
public class UpdateUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly UpdateUserHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateUserHandlerTests"/> class.
    /// Sets up the test dependencies and updates fake data generators.
    /// </summary>
    public UpdateUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = Substitute.For<IMapper>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _handler = new UpdateUserHandler(_userRepository, _mapper, _passwordHasher);
    }

    /// <summary>
    /// Tests that a valid user updated request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid user data When updating user Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = UpdateUserHandlerTestData.GenerateValidCommand();
        var existingUser = new User
        {
            Id = command.Id,
            Name = new Name("Old", "Name"),
            Address = new Address("OldCity", "OldStreet", 1, "11111-111", new Geolocation("0", "0")),
            Email = "old@email.com",
            Password = "oldpassword",
            Phone = "000000000",
            Status = DeveloperEvaluation.Domain.Enums.UserStatus.Active,
            Role = DeveloperEvaluation.Domain.Enums.UserRole.Customer,
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };

        var expectedHashedPassword = "hashedPassword";
        var expectedUpdatedAt = DateTime.UtcNow;

        _userRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(existingUser);
        _passwordHasher.HashPassword(command.Password).Returns(expectedHashedPassword);

        _userRepository.UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => callInfo.Arg<User>());

        var expectedResult = new UpdateUserResult
        {
            Id = existingUser.Id,
            Username = existingUser.Username,
            Email = command.Email,
            Phone = command.Phone,
            Status = command.Status,
            Role = command.Role,
            Name = new NameResult { Firstname = command.Name.Firstname, Lastname = command.Name.Lastname },
            Address = new AddressResult
            {
                City = command.Address.City,
                Street = command.Address.Street,
                Number = command.Address.Number,
                Zipcode = command.Address.Zipcode,
                Geolocation = new GeolocationResult
                {
                    Lat = command.Address.Geolocation.Lat,
                    Long = command.Address.Geolocation.Long
                }
            }
        };
        _mapper.Map<UpdateUserResult>(Arg.Any<User>()).Returns(expectedResult);

        // When
        var updateUserResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        updateUserResult.Should().NotBeNull();
        updateUserResult.Id.Should().Be(updateUserResult.Id);
        await _userRepository.Received(1).UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid user creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid user data When updating user Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new UpdateUserCommand(); // Empty command will fail validation

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that the password is hashed before saving the user.
    /// </summary>
    [Fact(DisplayName = "Given user updated request When handling Then password is hashed")]
    public async Task Handle_ValidRequest_HashesPassword()
    {
        // Given
        var command = UpdateUserHandlerTestData.GenerateValidCommand();
        var originalPassword = command.Password;
        const string hashedPassword = "h@shedPassw0rd";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Password = command.Password,
            Email = command.Email,
            Phone = command.Phone,
            Status = command.Status,
            Role = command.Role,
            Name = new Name(command.Name.Firstname, command.Name.Lastname),
            Address = new Address(command.Address.City, command.Address.Street, command.Address.Number, command.Address.Zipcode, new Geolocation(command.Address.Geolocation.Lat, command.Address.Geolocation.Long)),
        };

        _userRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(user);
        _userRepository.UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.HashPassword(originalPassword).Returns(hashedPassword);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _passwordHasher.Received(1).HashPassword(originalPassword);
        await _userRepository.Received(1).UpdateAsync(
            Arg.Is<User>(u => u.Password == hashedPassword),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the mapper is called with the correct command.
    /// </summary>
    [Fact(DisplayName = "Given valid user data When updating user Then updates user and returns mapped result")]
    public async Task Handle_ValidRequest_UpdatesUserAndReturnsMappedResult()
    {
        // Given
        var command = UpdateUserHandlerTestData.GenerateValidCommand();
        var existingUser = new User
        {
            Id = command.Id,
            Name = new Name("Old", "Name"),
            Address = new Address("OldCity", "OldStreet", 1, "11111-111", new Geolocation("0", "0")),
            Email = "old@email.com",
            Password = "oldpassword",
            Phone = "000000000",
            Status = DeveloperEvaluation.Domain.Enums.UserStatus.Active,
            Role = DeveloperEvaluation.Domain.Enums.UserRole.Customer,
            UpdatedAt = DateTime.UtcNow.AddDays(-1)
        };

        var expectedHashedPassword = "hashedPassword";
        var expectedUpdatedAt = DateTime.UtcNow;

        _userRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(existingUser);
        _passwordHasher.HashPassword(command.Password).Returns(expectedHashedPassword);
        _userRepository.UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(callInfo => callInfo.Arg<User>());

        var expectedResult = new UpdateUserResult
        {
            Id = existingUser.Id,
            Username = existingUser.Username,
            Email = command.Email,
            Phone = command.Phone,
            Status = command.Status,
            Role = command.Role,
            Name = new NameResult { Firstname = command.Name.Firstname, Lastname = command.Name.Lastname },
            Address = new AddressResult
            {
                City = command.Address.City,
                Street = command.Address.Street,
                Number = command.Address.Number,
                Zipcode = command.Address.Zipcode,
                Geolocation = new GeolocationResult
                {
                    Lat = command.Address.Geolocation.Lat,
                    Long = command.Address.Geolocation.Long
                }
            }
        };
        _mapper.Map<UpdateUserResult>(Arg.Any<User>()).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);

        await _userRepository.Received(1).UpdateAsync(
            Arg.Is<User>(u =>
                u.Id == command.Id &&
                u.Password == expectedHashedPassword &&
                u.Status == command.Status &&
                u.Role == command.Role &&
                u.Email == command.Email &&
                u.Name.Firstname == command.Name.Firstname &&
                u.Name.Lastname == command.Name.Lastname &&
                u.Phone == command.Phone &&
                u.Address.City == command.Address.City &&
                u.Address.Street == command.Address.Street &&
                u.Address.Number == command.Address.Number &&
                u.Address.Zipcode == command.Address.Zipcode &&
                u.Address.Geolocation.Lat == command.Address.Geolocation.Lat &&
                u.Address.Geolocation.Long == command.Address.Geolocation.Long
            ),
            Arg.Any<CancellationToken>());

        _mapper.Received(1).Map<UpdateUserResult>(Arg.Any<User>());
    }
}
