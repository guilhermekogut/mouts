using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Carts;

using FluentAssertions;

using FluentValidation;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts;

/// <summary>
/// Contains unit tests for the DeleteCartHandler class.
/// </summary>
public class DeleteCartHandlerTests
{
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly DeleteCartHandler _handler;

    public DeleteCartHandlerTests()
    {
        _handler = new DeleteCartHandler(_cartRepository);
    }

    [Fact(DisplayName = "Given valid cart id When deleting cart Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = DeleteCartHandlerTestData.GenerateValidCommand();
        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(new Cart()); // Simulate cart exists
        _cartRepository.DeleteAsync(command.Id, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Id.Should().Be(command.Id);
    }

    [Fact(DisplayName = "Given invalid cart id When deleting cart Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = DeleteCartHandlerTestData.GenerateInvalidCommand();

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact(DisplayName = "Given non-existent cart id When deleting cart Then returns not found response")]
    public async Task Handle_NonExistentCart_ReturnsNotFoundResponse()
    {
        // Given
        var command = DeleteCartHandlerTestData.GenerateValidCommand();
        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Cart?)null);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Id.Should().Be(command.Id);
        result.Message.Should().Be("Cart not found.");
    }
}