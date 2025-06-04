using Ambev.DeveloperEvaluation.Application.Carts.Common;
using Ambev.DeveloperEvaluation.Application.Carts.ListCarts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Carts;

using AutoMapper;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Carts;

/// <summary>
/// Contains unit tests for the ListCartsHandler class.
/// </summary>
public class ListCartsHandlerTests
{
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly ListCartsHandler _handler;

    public ListCartsHandlerTests()
    {
        _handler = new ListCartsHandler(_cartRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid request When listing carts Then returns paged result")]
    public async Task Handle_ValidRequest_ReturnsPagedResult()
    {
        // Given
        var command = ListCartsHandlerTestData.GenerateValidCommand();
        var carts = new List<Cart>
        {
            new Cart { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Date = DateTime.UtcNow, Products = new List<CartProduct>() }
        }.AsQueryable();

        _cartRepository.Query().Returns(carts);
        _mapper.Map<IEnumerable<ListCartItemResult>>(Arg.Any<IEnumerable<Cart>>()).Returns(new List<ListCartItemResult>());

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
        result.TotalItems.Should().Be(1);
        result.CurrentPage.Should().Be(command.Page);
        result.TotalPages.Should().Be(1);
    }

    [Fact(DisplayName = "Given empty result When listing carts Then returns empty data")]
    public async Task Handle_EmptyResult_ReturnsEmptyData()
    {
        // Given
        var command = ListCartsHandlerTestData.GenerateValidCommand();
        var carts = new List<Cart>().AsQueryable();

        _cartRepository.Query().Returns(carts);
        _mapper.Map<IEnumerable<ListCartItemResult>>(Arg.Any<IEnumerable<Cart>>()).Returns(new List<ListCartItemResult>());

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Data.Should().BeEmpty();
        result.TotalItems.Should().Be(0);
        result.TotalPages.Should().Be(0);
    }

    [Fact(DisplayName = "Given filter by userId When listing carts Then only user carts are returned")]
    public async Task Handle_FilterByUserId_ReturnsOnlyUserCarts()
    {
        // Given
        var userId = Guid.NewGuid();
        var command = ListCartsHandlerTestData.GenerateValidCommand();
        command.UserId = userId;

        var carts = new List<Cart>
        {
            new Cart { Id = Guid.NewGuid(), UserId = userId, Date = DateTime.UtcNow, Products = new List<CartProduct>() },
            new Cart { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Date = DateTime.UtcNow, Products = new List<CartProduct>() }
        }.AsQueryable();

        _cartRepository.Query().Returns(carts);
        _mapper.Map<IEnumerable<ListCartItemResult>>(Arg.Any<IEnumerable<Cart>>())
            .Returns(callInfo =>
            {
                var filtered = ((IEnumerable<Cart>)callInfo.ArgAt<IEnumerable<Cart>>(0)).Where(c => c.UserId == userId);
                return filtered.Select(c => new ListCartItemResult { Id = c.Id, UserId = c.UserId, Date = c.Date, Products = new List<CartProductItemResult>() });
            });

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Data.Should().OnlyContain(x => x.UserId == userId);
    }

    [Fact(DisplayName = "Given page size When listing carts Then returns correct pagination")]
    public async Task Handle_Pagination_WorksCorrectly()
    {
        // Given
        var command = ListCartsHandlerTestData.GenerateValidCommand();
        command.Page = 2;
        command.Size = 1;

        var carts = new List<Cart>
        {
            new Cart { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Date = DateTime.UtcNow, Products = new List<CartProduct>() },
            new Cart { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Date = DateTime.UtcNow, Products = new List<CartProduct>() }
        }.AsQueryable();

        _cartRepository.Query().Returns(carts);
        _mapper.Map<IEnumerable<ListCartItemResult>>(Arg.Any<IEnumerable<Cart>>())
            .Returns(callInfo =>
            {
                var paged = ((IEnumerable<Cart>)callInfo.ArgAt<IEnumerable<Cart>>(0)).Skip((command.Page - 1) * command.Size).Take(command.Size);
                return paged.Select(c => new ListCartItemResult { Id = c.Id, UserId = c.UserId, Date = c.Date, Products = new List<CartProductItemResult>() });
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