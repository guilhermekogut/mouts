using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Users;

using AutoMapper;

using FluentAssertions;

using FluentValidation;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users;

/// <summary>
/// Contains unit tests for the <see cref="GetUserHandler"/> class.
/// </summary>
public class GetUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly GetUserHandler _handler;

    public GetUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetUserHandler(_userRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid user id When getting user Then returns mapped result")]
    public async Task Handle_ValidRequest_ReturnsMappedResult()
    {
        // Given
        var command = GetUserHandlerTestData.GenerateValidCommand();
        var user = new User
        {
            Id = command.Id,
            Name = new DeveloperEvaluation.Domain.ValueObjects.Name("John", "Doe"),
            Address = new DeveloperEvaluation.Domain.ValueObjects.Address("City", "Street", 123, "00000-000", new DeveloperEvaluation.Domain.ValueObjects.Geolocation("1.23", "4.56")),
            Email = "john.doe@email.com",
            Phone = "+5511999999999",
            Status = DeveloperEvaluation.Domain.Enums.UserStatus.Active,
            Role = DeveloperEvaluation.Domain.Enums.UserRole.Customer,
            Username = "johndoe"
        };
        var expectedResult = GetUserHandlerTestData.GenerateValidResult();
        expectedResult.Id = user.Id;

        _userRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(user);
        _mapper.Map<GetUserResult>(user).Returns(expectedResult);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
        await _userRepository.Received(1).GetByIdAsync(command.Id, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<GetUserResult>(user);
    }

    [Fact(DisplayName = "Given invalid user id When getting user Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new GetUserCommand(Guid.Empty); // Invalid Id

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given non-existent user id When getting user Then throws key not found exception")]
    public async Task Handle_UserNotFound_ThrowsKeyNotFoundException()
    {
        // Given
        var command = GetUserHandlerTestData.GenerateValidCommand();
        _userRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((User)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"User with ID {command.Id} not found");
    }
}