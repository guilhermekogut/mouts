using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Users;

using FluentAssertions;

using FluentValidation;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Users;

/// <summary>
/// Contains unit tests for the <see cref="DeleteUserHandler"/>.
/// </summary>
public class DeleteUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly DeleteUserHandler _handler;

    public DeleteUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _handler = new DeleteUserHandler(_userRepository);
    }

    [Fact(DisplayName = "Given valid user id When deleting user Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = DeleteUserHandlerTestData.GenerateValidCommand();
        _userRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(true);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        await _userRepository.Received(1).DeleteAsync(command.Id, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given invalid user id When deleting user Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new DeleteUserCommand(Guid.Empty); // Invalid Id

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given non-existent user id When deleting user Then throws key not found exception")]
    public async Task Handle_UserNotFound_ThrowsKeyNotFoundException()
    {
        // Given
        var command = DeleteUserHandlerTestData.GenerateValidCommand();
        _userRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(false);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"User with ID {command.Id} not found");
    }
}