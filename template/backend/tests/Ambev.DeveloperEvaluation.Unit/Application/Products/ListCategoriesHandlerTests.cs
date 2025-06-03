using Ambev.DeveloperEvaluation.Application.Products.ListCategories;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Products;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the ListCategoriesHandler class.
/// </summary>
public class ListCategoriesHandlerTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly ListCategoriesHandler _handler;

    public ListCategoriesHandlerTests()
    {
        _handler = new ListCategoriesHandler(_productRepository);
    }

    [Fact(DisplayName = "Given valid request When listing categories Then returns all categories")]
    public async Task Handle_ValidRequest_ReturnsAllCategories()
    {
        // Given
        var command = ListCategoriesHandlerTestData.GenerateValidCommand();
        var categories = new List<string> { "Electronics", "Books", "Toys" };
        _productRepository.GetCategoriesAsync(Arg.Any<CancellationToken>()).Returns(categories);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Categories.Should().BeEquivalentTo(categories);
    }

    [Fact(DisplayName = "Given filter When listing categories Then returns filtered categories")]
    public async Task Handle_FilteredRequest_ReturnsFilteredCategories()
    {
        // Given
        var command = ListCategoriesHandlerTestData.GenerateFilteredCommand("Book*");
        var categories = new List<string> { "Electronics", "Books", "Toys" };
        _productRepository.GetCategoriesAsync(Arg.Any<CancellationToken>()).Returns(categories);

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Categories.Should().Contain("Books");
    }
}