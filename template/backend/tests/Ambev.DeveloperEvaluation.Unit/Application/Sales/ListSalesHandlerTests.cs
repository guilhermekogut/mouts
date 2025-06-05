using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;

using AutoMapper;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the ListSalesHandler class.
/// </summary>
public class ListSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly ListSalesHandler _handler;

    public ListSalesHandlerTests()
    {
        _handler = new ListSalesHandler(_saleRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid request When listing sales Then returns paged result")]
    public async Task Handle_ValidRequest_ReturnsPagedResult()
    {
        // Given
        var command = ListSalesHandlerTestData.GenerateValidCommand();
        var sales = new List<Sale>
        {
            new Sale { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Date = DateTime.UtcNow, Items = new List<SaleItem>() }
        }.AsQueryable();

        _saleRepository.Query().Returns(sales);
        _mapper.Map<IEnumerable<ListSaleItemResult>>(Arg.Any<IEnumerable<Sale>>()).Returns(new List<ListSaleItemResult>());

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
        result.TotalItems.Should().Be(1);
        result.CurrentPage.Should().Be(command.Page);
        result.TotalPages.Should().Be(1);
    }

    [Fact(DisplayName = "Given empty result When listing sales Then returns empty data")]
    public async Task Handle_EmptyResult_ReturnsEmptyData()
    {
        // Given
        var command = ListSalesHandlerTestData.GenerateValidCommand();
        var sales = new List<Sale>().AsQueryable();

        _saleRepository.Query().Returns(sales);
        _mapper.Map<IEnumerable<ListSaleItemResult>>(Arg.Any<IEnumerable<Sale>>()).Returns(new List<ListSaleItemResult>());

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Data.Should().BeEmpty();
        result.TotalItems.Should().Be(0);
        result.TotalPages.Should().Be(0);
    }

    [Fact(DisplayName = "Given filter by customerId When listing sales Then only customer sales are returned")]
    public async Task Handle_FilterByCustomerId_ReturnsOnlyCustomerSales()
    {
        // Given
        var customerId = Guid.NewGuid();
        var command = ListSalesHandlerTestData.GenerateValidCommand();
        command.CustomerId = customerId;

        var sales = new List<Sale>
        {
            new Sale { Id = Guid.NewGuid(), CustomerId = customerId, Date = DateTime.UtcNow, Items = new List<SaleItem>() },
            new Sale { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Date = DateTime.UtcNow, Items = new List<SaleItem>() }
        }.AsQueryable();

        _saleRepository.Query().Returns(sales);
        _mapper.Map<IEnumerable<ListSaleItemResult>>(Arg.Any<IEnumerable<Sale>>())
            .Returns(callInfo =>
            {
                var filtered = ((IEnumerable<Sale>)callInfo.ArgAt<IEnumerable<Sale>>(0)).Where(s => s.CustomerId == customerId);
                return filtered.Select(s => new ListSaleItemResult { Id = s.Id, CustomerId = s.CustomerId, Date = s.Date, Items = new List<SaleItemResult>() });
            });

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Data.Should().OnlyContain(x => x.CustomerId == customerId);
    }

    [Fact(DisplayName = "Given page size When listing sales Then returns correct pagination")]
    public async Task Handle_Pagination_WorksCorrectly()
    {
        // Given
        var command = ListSalesHandlerTestData.GenerateValidCommand();
        command.Page = 2;
        command.Size = 1;

        var sales = new List<Sale>
        {
            new Sale { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Date = DateTime.UtcNow, Items = new List<SaleItem>() },
            new Sale { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Date = DateTime.UtcNow, Items = new List<SaleItem>() }
        }.AsQueryable();

        _saleRepository.Query().Returns(sales);
        _mapper.Map<IEnumerable<ListSaleItemResult>>(Arg.Any<IEnumerable<Sale>>())
            .Returns(callInfo =>
            {
                var paged = ((IEnumerable<Sale>)callInfo.ArgAt<IEnumerable<Sale>>(0)).Skip((command.Page - 1) * command.Size).Take(command.Size);
                return paged.Select(s => new ListSaleItemResult { Id = s.Id, CustomerId = s.CustomerId, Date = s.Date, Items = new List<SaleItemResult>() });
            });

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.CurrentPage.Should().Be(2);
        result.TotalItems.Should().Be(2);
        result.TotalPages.Should().Be(2);
    }
}