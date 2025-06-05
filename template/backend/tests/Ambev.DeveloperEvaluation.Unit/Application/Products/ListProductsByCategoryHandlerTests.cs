using Ambev.DeveloperEvaluation.Application.Products.Common;
using Ambev.DeveloperEvaluation.Application.Products.ListProductsByCategory;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;

using AutoMapper;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the ListProductsByCategoryHandler class.
/// </summary>
public class ListProductsByCategoryHandlerTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly ListProductsByCategoryHandler _handler;

    public ListProductsByCategoryHandlerTests()
    {
        _handler = new ListProductsByCategoryHandler(_productRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid category When listing products Then returns paged result")]
    public async Task Handle_ValidRequest_ReturnsPagedResult()
    {
        // Given
        var command = ListProductsByCategoryHandlerTestData.GenerateValidCommand();
        var products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Title = "Test", Price = 10, Description = "Desc", Category = command.Category, Image = "img" }
        }.AsQueryable();

        _productRepository.QueryByCategory(command.Category).Returns(products);
        _mapper.Map<IEnumerable<ProductItemResult>>(Arg.Any<IEnumerable<Product>>()).Returns(new List<ProductItemResult>());

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
    }
}