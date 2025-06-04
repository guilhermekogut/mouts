using Ambev.DeveloperEvaluation.Domain.Entities;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Unit tests for the CartProduct entity.
/// </summary>
public class CartProductTests
{
    [Fact(DisplayName = "CartProduct constructor should set properties")]
    public void Given_ValidArguments_When_Constructed_Then_PropertiesAreSet()
    {
        // Arrange
        var productId = Guid.NewGuid();
        int quantity = 5;

        // Act
        var cartProduct = new CartProduct(productId, quantity);

        // Assert
        Assert.Equal(productId, cartProduct.ProductId);
        Assert.Equal(quantity, cartProduct.Quantity);
    }
}