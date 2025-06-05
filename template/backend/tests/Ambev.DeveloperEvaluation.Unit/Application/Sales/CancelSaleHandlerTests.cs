using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
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

public class CancelSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly IValidator<CancelSaleCommand> _validator = Substitute.For<IValidator<CancelSaleCommand>>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly CancelSaleHandler _handler;

    public CancelSaleHandlerTests()
    {
        _handler = new CancelSaleHandler(_saleRepository, _mapper, _validator, _mediator);
    }

    [Fact(DisplayName = "Given valid sale data When cancelling sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        var command = CancelSaleHandlerTestData.GenerateValidCommand();
        var sale = CancelSaleHandlerTestData.GenerateSaleFromCommand(command);
        var result = CancelSaleHandlerTestData.GenerateResultFromSale(sale);

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);

        _mapper.Map<CancelSaleResult>(sale).Returns(callInfo =>
        {
            var s = (Sale)callInfo[0];
            return CancelSaleHandlerTestData.GenerateResultFromSale(s);
        });

        var cancelResult = await _handler.Handle(command, CancellationToken.None);

        cancelResult.Should().NotBeNull();
        cancelResult.Id.Should().Be(sale.Id);
        cancelResult.Items.Should().HaveCount(sale.Items.Count);
        cancelResult.Cancelled.Should().BeTrue();
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
        await _mediator.Received(1).Publish(Arg.Any<SaleCancelledEvent>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given non-existent sale When cancelling sale Then throws invalid operation exception")]
    public async Task Handle_SaleNotFound_ThrowsInvalidOperationException()
    {
        var command = CancelSaleHandlerTestData.GenerateValidCommand();

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns((Sale?)null);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Sale not found.");
    }

    [Fact(DisplayName = "Given sale not owned by user When cancelling sale Then throws unauthorized access exception")]
    public async Task Handle_SaleNotOwnedByUser_ThrowsUnauthorizedAccessException()
    {
        var command = CancelSaleHandlerTestData.GenerateValidCommand();
        var sale = CancelSaleHandlerTestData.GenerateSaleFromCommand(command);
        sale.CustomerId = Guid.NewGuid(); // Different user

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        _saleRepository.GetByIdAsync(command.SaleId, Arg.Any<CancellationToken>()).Returns(sale);

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Sale does not belong to the user.");
    }

    [Fact(DisplayName = "Given invalid command When cancelling sale Then throws validation exception")]
    public async Task Handle_InvalidCommand_ThrowsValidationException()
    {
        var command = CancelSaleHandlerTestData.GenerateCommandWithInvalidSaleId();

        _validator.ValidateAsync(command, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult(new[] { new ValidationFailure(nameof(command.SaleId), "SaleId is required.") }));

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*SaleId is required*");
    }
}