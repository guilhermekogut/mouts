using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;

using AutoMapper;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the GetProductHandler class.
/// </summary>
public class GetProductHandlerTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IMapper _mapper = Substitute.For<IMapper>();
    private readonly GetProductHandler _handler;

    public GetProductHandlerTests()
    {
        _handler = new GetProductHandler(_productRepository, _mapper);
    }

    [Fact(DisplayName = "Given valid product id When getting product Then returns product result")]
    public async Task Handle_ValidRequest_ReturnsProductResult()
    {
        // Given
        var command = GetProductHandlerTestData.GenerateValidCommand();
        var product = new Product { Id = command.Id, Title = "Test", Price = 10, Description = "Desc", Category = "Cat", Image = "img" };
        var result = new GetProductResult { Id = product.Id, Title = product.Title, Price = product.Price, Description = product.Description, Category = product.Category, Image = product.Image };

        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(product);
        _mapper.Map<GetProductResult>(product).Returns(result);

        // When
        var getProductResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        getProductResult.Should().NotBeNull();
        getProductResult.Id.Should().Be(product.Id);
    }

    [Fact(DisplayName = "Given non-existent product id When getting product Then throws invalid operation exception")]
    public async Task Handle_NonExistentProduct_ThrowsInvalidOperationException()
    {
        // Given
        var command = GetProductHandlerTestData.GenerateValidCommand();
        _productRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Product)null);

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}