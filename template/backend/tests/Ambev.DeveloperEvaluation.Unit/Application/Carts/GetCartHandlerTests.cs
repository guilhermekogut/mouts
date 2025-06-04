using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Carts;

using AutoMapper;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts;

/// <summary>
/// Contains unit tests for the GetCartHandler class.
/// </summary>
public class GetCartHandlerTests
{
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly GetCartHandler _handler;

    public GetCartHandlerTests()
    {
        _handler = new GetCartHandler(_cartRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid cart id When getting cart Then returns cart result")]
    public async Task Handle_ValidRequest_ReturnsCartResult()
    {
        // Given
        var command = GetCartHandlerTestData.GenerateValidCommand();
        var cart = new Cart { Id = command.Id, UserId = Guid.NewGuid(), Date = DateTime.UtcNow, Products = new List<CartProduct>() };
        var result = new GetCartResult { Id = cart.Id, UserId = cart.UserId, Date = cart.Date, Products = new List<CartProductItemResult>() };

        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(cart);
        _mapper.Map<GetCartResult>(cart).Returns(result);

        // When
        var getCartResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getCartResult.Should().NotBeNull();
        getCartResult.Id.Should().Be(cart.Id);
        getCartResult.UserId.Should().Be(cart.UserId);
    }

    [Fact(DisplayName = "Given non-existent cart id When getting cart Then throws invalid operation exception")]
    public async Task Handle_NonExistentCart_ThrowsInvalidOperationException()
    {
        // Given
        var command = GetCartHandlerTestData.GenerateValidCommand();
        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Cart?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}