using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;

using AutoMapper;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the GetSaleHandler class.
/// </summary>
public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly GetSaleHandler _handler;

    public GetSaleHandlerTests()
    {
        _handler = new GetSaleHandler(_saleRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid sale id When getting sale Then returns sale result")]
    public async Task Handle_ValidRequest_ReturnsSaleResult()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale { Id = command.Id, CustomerId = Guid.NewGuid(), Date = DateTime.UtcNow, Items = new List<SaleItem>() };
        var result = new GetSaleResult { Id = sale.Id, CustomerId = sale.CustomerId, Date = sale.Date, Items = new List<SaleItemResult>() };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(result);

        // When
        var getSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getSaleResult.Should().NotBeNull();
        getSaleResult.Id.Should().Be(sale.Id);
        getSaleResult.CustomerId.Should().Be(sale.CustomerId);
    }

    [Fact(DisplayName = "Given non-existent sale id When getting sale Then throws invalid operation exception")]
    public async Task Handle_NonExistentSale_ThrowsInvalidOperationException()
    {
        // Given
        var command = GetSaleHandlerTestData.GenerateValidCommand();
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Sale?)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}