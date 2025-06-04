using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Repositories.Results;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Carts;

using AutoMapper;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts;

/// <summary>
/// Contains unit tests for the CreateCartHandler class.
/// </summary>
public class CreateCartHandlerTests
{
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly CreateCartHandler _handler;

    public CreateCartHandlerTests()
    {
        _handler = new CreateCartHandler(_cartRepository, _mapper, _productRepository);
    }

    [Fact(DisplayName = "Given valid cart data When creating cart Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateCartHandlerTestData.GenerateValidCommand();
        var cart = CreateCartHandlerTestData.GenerateCartFromCommand(command);
        var result = CreateCartHandlerTestData.GenerateResultFromCart(cart);

        _cartRepository.GetByUserIdAsync(command.UserId, Arg.Any<CancellationToken>()).Returns((Cart?)null);
        _mapper.Map<Cart>(command).Returns(cart);
        _mapper.Map<CreateCartResult>(cart).Returns(result);
        _cartRepository.AddAsync(cart, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var existenceItems = command.Products
        .Select(p => new ProductExistenceItem(p.ProductId, true))
        .ToList();
        _productRepository.CheckExistenceAsync(
            Arg.Any<IEnumerable<Guid>>(),
            Arg.Any<CancellationToken>())
            .Returns(new ProductExistenceResult(existenceItems));

        // When
        var createCartResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createCartResult.Should().NotBeNull();
        createCartResult.UserId.Should().Be(cart.UserId);
        createCartResult.Products.Should().HaveCount(cart.Products.Count);
        await _cartRepository.Received(1).AddAsync(cart, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given user with active cart When creating cart Then throws invalid operation exception")]
    public async Task Handle_UserAlreadyHasCart_ThrowsInvalidOperationException()
    {
        // Given
        var command = CreateCartHandlerTestData.GenerateValidCommand();
        var existingCart = CreateCartHandlerTestData.GenerateCartFromCommand(command);

        _cartRepository.GetByUserIdAsync(command.UserId, Arg.Any<CancellationToken>()).Returns(existingCart);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("User already has an active cart.");
    }

    [Fact(DisplayName = "Given invalid product quantity When creating cart Then throws invalid operation exception")]
    public async Task Handle_InvalidProductQuantity_ThrowsInvalidOperationException()
    {
        // Given
        var command = CreateCartHandlerTestData.GenerateCommandWithInvalidQuantity();

        _cartRepository.GetByUserIdAsync(command.UserId, Arg.Any<CancellationToken>()).Returns((Cart?)null);

        var existenceItems = command.Products
        .Select(p => new ProductExistenceItem(p.ProductId, true))
        .ToList();
        _productRepository.CheckExistenceAsync(
            Arg.Any<IEnumerable<Guid>>(),
            Arg.Any<CancellationToken>())
            .Returns(new ProductExistenceResult(existenceItems));

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*quantity*");
    }

    [Fact(DisplayName = "Given invalid product id When creating cart Then throws invalid operation exception")]
    public async Task Handle_InvalidProductId_ThrowsInvalidOperationException()
    {
        // Given
        var invalidProductId = Guid.NewGuid();
        var command = CreateCartHandlerTestData.GenerateCommandWithInvalidProductId(invalidProductId);

        _cartRepository.GetByUserIdAsync(command.UserId, Arg.Any<CancellationToken>()).Returns((Cart?)null);

        // Simula apenas o primeiro produto como inexistente
        var existenceItems = command.Products
            .Select((p, idx) => new ProductExistenceItem(p.ProductId, idx != 0))
            .ToList();
        _productRepository.CheckExistenceAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new ProductExistenceResult(existenceItems));

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"The following product IDs do not exist: {invalidProductId}");
    }
}