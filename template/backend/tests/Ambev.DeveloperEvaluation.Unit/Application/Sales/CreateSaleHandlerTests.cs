using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Repositories.Results;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;

using AutoMapper;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using MediatR;

using Microsoft.Extensions.Logging;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the CreateSaleHandler class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IValidator<CreateSaleCommand> _validator = Substitute.For<IValidator<CreateSaleCommand>>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly ILogger<CreateSaleHandler> _logger = Substitute.For<ILogger<CreateSaleHandler>>();
    private readonly CreateSaleHandler _handler;

    public CreateSaleHandlerTests()
    {
        _handler = new CreateSaleHandler(
            _cartRepository,
            _saleRepository,
            _productRepository,
            _mapper,
            _validator,
            _userRepository,
            _logger, // logger pode continuar null se não for usado
            _mediator
        );
    }

    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var cart = new Cart(command.AuthenticatedUserId, DateTime.UtcNow, new[] { new CartProduct(Guid.NewGuid(), 5) });
        var user = new User { Id = command.AuthenticatedUserId, Username = "user", Name = new Name { Firstname = "Test", Lastname = "User" } };
        var product = new Product { Id = cart.Products[0].ProductId, Title = "Product", Price = 10m };
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            CustomerId = user.Id,
            CustomerName = "Test User",
            Date = DateTime.UtcNow,
            Items = new List<SaleItem>
            {
                new SaleItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    ProductName = product.Title,
                    Quantity = 5,
                    UnitPrice = 10m,
                    Discount = 5m,
                    Total = 45m
                }
            },
            TotalAmount = 45m
        };
        var result = new CreateSaleResult
        {
            Id = sale.Id,
            CustomerId = sale.CustomerId,
            CustomerName = sale.CustomerName,
            Date = sale.Date,
            Items = sale.Items.Select(i => new SaleItemResult
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                Discount = i.Discount,
                Total = i.Total
            }).ToList(),
            TotalAmount = sale.TotalAmount
        };

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());
        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>()).Returns(cart);
        _userRepository.GetByIdAsync(cart.UserId, Arg.Any<CancellationToken>()).Returns(user);
        _productRepository.CheckExistenceAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new ProductExistenceResult(new[] { new ProductExistenceItem(product.Id, true) }));
        _productRepository.Query().Returns(new[] { product }.AsQueryable());
        _saleRepository.AddAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _cartRepository.DeleteAsync(cart.Id, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
        _mapper.Map<CreateSaleResult>(Arg.Any<Sale>()).Returns(result);

        // When
        var saleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        saleResult.Should().NotBeNull();
        saleResult.CustomerId.Should().Be(user.Id);
        saleResult.Items.Should().HaveCount(1);
        await _saleRepository.Received(1).AddAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
        await _cartRepository.Received(1).DeleteAsync(cart.Id, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given invalid cart id When creating sale Then throws invalid operation exception")]
    public async Task Handle_InvalidCartId_ThrowsInvalidOperationException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());
        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>()).Returns((Cart?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cart not found.");
    }

    [Fact(DisplayName = "Given cart not owned by user When creating sale Then throws unauthorized access exception")]
    public async Task Handle_CartNotOwnedByUser_ThrowsUnauthorizedAccessException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var cart = new Cart(Guid.NewGuid(), DateTime.UtcNow, new[] { new CartProduct(Guid.NewGuid(), 2) });
        var user = new User { Id = Guid.NewGuid(), Username = "otheruser" };

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());
        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>()).Returns(cart);
        _userRepository.GetByIdAsync(cart.UserId, Arg.Any<CancellationToken>()).Returns(user);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Cart does not belong to the user.");
    }

    [Fact(DisplayName = "Given user not found When creating sale Then throws invalid operation exception")]
    public async Task Handle_UserNotFound_ThrowsInvalidOperationException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var cart = new Cart(command.AuthenticatedUserId, DateTime.UtcNow, new[] { new CartProduct(Guid.NewGuid(), 2) });

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());
        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>()).Returns(cart);
        _userRepository.GetByIdAsync(cart.UserId, Arg.Any<CancellationToken>()).Returns((User?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("User not found.");
    }

    [Fact(DisplayName = "Given non-existent product When creating sale Then throws invalid operation exception")]
    public async Task Handle_NonExistentProduct_ThrowsInvalidOperationException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var cart = new Cart(command.AuthenticatedUserId, DateTime.UtcNow, new[] { new CartProduct(Guid.NewGuid(), 2) });
        var user = new User { Id = command.AuthenticatedUserId, Username = "user" };

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>()).Returns(new ValidationResult());
        _cartRepository.GetByIdAsync(command.CartId, Arg.Any<CancellationToken>()).Returns(cart);
        _userRepository.GetByIdAsync(cart.UserId, Arg.Any<CancellationToken>()).Returns(user);
        _productRepository.CheckExistenceAsync(Arg.Any<IEnumerable<Guid>>(), Arg.Any<CancellationToken>())
            .Returns(new ProductExistenceResult(new[] { new ProductExistenceItem(cart.Products[0].ProductId, false) }));

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"The following product IDs do not exist: {cart.Products[0].ProductId}");
    }

    [Fact(DisplayName = "Given invalid command When creating sale Then throws validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        // Given
        var command = CreateSaleHandlerTestData.GenerateCommandWithInvalidCartId();
        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[] { new ValidationFailure(nameof(command.CartId), "CartId is required.") }));

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*CartId is required*");
    }
}