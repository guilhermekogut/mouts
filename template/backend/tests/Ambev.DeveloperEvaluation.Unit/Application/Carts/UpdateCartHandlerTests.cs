using Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Carts;

using AutoMapper;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts;

public class UpdateCartHandlerTests
{
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly UpdateCartHandler _handler;
    private readonly IValidator<UpdateCartCommand> _validator = Substitute.For<IValidator<UpdateCartCommand>>();

    public UpdateCartHandlerTests()
    {
        _handler = new UpdateCartHandler(_cartRepository, _mapper, _validator);
    }

    [Fact(DisplayName = "Given valid cart data When updating cart Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        var command = UpdateCartHandlerTestData.GenerateValidCommand();
        var cart = UpdateCartHandlerTestData.GenerateCartFromCommand(command);
        var result = UpdateCartHandlerTestData.GenerateResultFromCart(cart);

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(cart);
        _mapper.Map<UpdateCartResult>(cart).Returns(result);

        var updateResult = await _handler.Handle(command, CancellationToken.None);

        updateResult.Should().NotBeNull();
        updateResult.UserId.Should().Be(cart.UserId);
        updateResult.Products.Should().HaveCount(cart.Products.Count);
        await _cartRepository.Received(1).UpdateAsync(cart, Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given non-existent cart When updating cart Then throws invalid operation exception")]
    public async Task Handle_CartNotFound_ThrowsInvalidOperationException()
    {
        var command = UpdateCartHandlerTestData.GenerateValidCommand();

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Cart?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cart not found.");
    }

    [Fact(DisplayName = "Given UserId different from cart When updating cart Then throws invalid operation exception")]
    public async Task Handle_UserIdChanged_ThrowsInvalidOperationException()
    {
        var command = UpdateCartHandlerTestData.GenerateValidCommand();
        var cart = UpdateCartHandlerTestData.GenerateCartFromCommand(command);
        cart.UserId = Guid.NewGuid(); // Different user

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(cart);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("UserId cannot be changed.");
    }

    [Fact(DisplayName = "Given invalid product quantity When updating cart Then throws validation exception")]
    public async Task Handle_InvalidProductQuantity_ThrowsValidationException()
    {
        var command = UpdateCartHandlerTestData.GenerateCommandWithInvalidQuantity();
        var cart = UpdateCartHandlerTestData.GenerateCartFromCommand(command);

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[] { new ValidationFailure("Products[0].Quantity", "Quantity must be greater than zero.") }));

        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(cart);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Quantity must be greater than zero*");
    }

    [Fact(DisplayName = "Given duplicate products When updating cart Then throws validation exception")]
    public async Task Handle_DuplicateProducts_ThrowsValidationException()
    {
        var command = UpdateCartHandlerTestData.GenerateCommandWithDuplicateProducts();
        var cart = UpdateCartHandlerTestData.GenerateCartFromCommand(command);

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[] { new ValidationFailure("Products", "Duplicate products are not allowed in the cart.") }));

        _cartRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(cart);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Duplicate products are not allowed in the cart*");
    }
}