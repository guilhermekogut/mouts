using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;

using AutoMapper;

using FluentAssertions;

using FluentValidation;
using FluentValidation.Results;

using MediatR;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IValidator<UpdateSaleCommand> _validator = Substitute.For<IValidator<UpdateSaleCommand>>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly UpdateSaleHandler _handler;

    public UpdateSaleHandlerTests()
    {
        _handler = new UpdateSaleHandler(_saleRepository, _mapper, _validator, _mediator);
    }

    [Fact(DisplayName = "Given valid sale data When updating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        var sale = UpdateSaleHandlerTestData.GenerateSaleFromCommand(command);
        var result = UpdateSaleHandlerTestData.GenerateResultFromSale(sale);

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<UpdateSaleResult>(sale).Returns(result);

        var updateResult = await _handler.Handle(command, CancellationToken.None);

        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(sale.Id);
        updateResult.Items.Should().HaveCount(sale.Items.Count);
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
        await _mediator.Received(1).Publish(Arg.Any<SaleUpdatedEvent>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given non-existent sale When updating sale Then throws invalid operation exception")]
    public async Task Handle_SaleNotFound_ThrowsInvalidOperationException()
    {
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns((Sale?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Sale not found.");
    }

    [Fact(DisplayName = "Given sale not owned by user When updating sale Then throws unauthorized access exception")]
    public async Task Handle_SaleNotOwnedByUser_ThrowsUnauthorizedAccessException()
    {
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        var sale = UpdateSaleHandlerTestData.GenerateSaleFromCommand(command);
        sale.CustomerId = Guid.NewGuid(); // Different user

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Sale does not belong to the user.");
    }

    [Fact(DisplayName = "Given sale item already cancelled When updating sale Then throws invalid operation exception")]
    public async Task Handle_ItemAlreadyCancelled_ThrowsInvalidOperationException()
    {
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        var sale = UpdateSaleHandlerTestData.GenerateSaleFromCommand(command, itemCancelled: true);

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Sale item not found or already cancelled.");
    }

    [Fact(DisplayName = "Given invalid command When updating sale Then throws validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        var command = UpdateSaleHandlerTestData.GenerateCommandWithInvalidProduct();

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[] { new ValidationFailure(nameof(command.ProductId), "ProductId is required.") }));

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*ProductId is required*");
    }
}