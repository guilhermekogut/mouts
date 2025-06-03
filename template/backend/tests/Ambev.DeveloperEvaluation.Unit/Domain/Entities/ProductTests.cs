using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Unit tests for the Product entity.
/// </summary>
public class ProductTests
{
    [Fact(DisplayName = "Validation should pass for valid product data")]
    public void Given_ValidProduct_When_Validated_Then_ShouldNotThrow()
    {
        // Arrange
        var product = ProductTestData.GenerateValidProduct();

        // Act & Assert
        product.Validate();
    }

    [Fact(DisplayName = "Validation should fail for invalid product data")]
    public void Given_InvalidProduct_When_Validated_Then_ShouldThrowArgumentException()
    {
        // Arrange
        var product = ProductTestData.GenerateInvalidProduct();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => product.Validate());
    }
}