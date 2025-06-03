using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;

using AutoMapper;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the ListProductsHandler class.
/// </summary>
public class ListProductsHandlerTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly ListProductsHandler _handler;

    public ListProductsHandlerTests()
    {
        _handler = new ListProductsHandler(_productRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid request When listing products Then returns paged result")]
    public async Task Handle_ValidRequest_ReturnsPagedResult()
    {
        // Given
        var command = ListProductsHandlerTestData.GenerateValidCommand();
        var products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Title = "Test", Price = 10, Description = "Desc", Category = "Cat", Image = "img" }
        }.AsQueryable();

        _productRepository.Query().Returns(products);
        _mapper.Map<IEnumerable<ProductItemResult>>(Arg.Any<IEnumerable<Product>>()).Returns(new List<ProductItemResult>());

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
    }
}