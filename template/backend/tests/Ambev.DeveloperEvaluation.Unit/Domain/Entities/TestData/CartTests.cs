using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Unit tests for the Cart entity.
/// </summary>
public class CartTests
{
    [Fact(DisplayName = "AddOrUpdateProduct should add a new product")]
    public void Given_EmptyCart_When_AddingProduct_Then_ProductIsAdded()
    {
        // Arrange
        var cart = CartTestData.GenerateEmptyCart();
        var product = CartProductTestData.GenerateValidCartProduct();

        // Act
        cart.AddOrUpdateProduct(product.ProductId, product.Quantity);

        // Assert
        Assert.Single(cart.Products);
        Assert.Equal(product.ProductId, cart.Products[0].ProductId);
        Assert.Equal(product.Quantity, cart.Products[0].Quantity);
    }

    [Fact(DisplayName = "AddOrUpdateProduct should update quantity if product exists")]
    public void Given_CartWithProduct_When_UpdatingQuantity_Then_QuantityIsUpdated()
    {
        // Arrange
        var cart = CartTestData.GenerateCartWithOneProduct();
        var product = cart.Products[0];
        int newQuantity = product.Quantity + 1;

        // Act
        cart.AddOrUpdateProduct(product.ProductId, newQuantity);

        // Assert
        Assert.Single(cart.Products);
        Assert.Equal(newQuantity, cart.Products[0].Quantity);
    }

    [Fact(DisplayName = "AddOrUpdateProduct should throw if quantity is invalid")]
    public void Given_InvalidQuantity_When_AddingOrUpdating_Then_ShouldThrow()
    {
        // Arrange
        var cart = CartTestData.GenerateEmptyCart();
        var productId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => cart.AddOrUpdateProduct(productId, 0));
        Assert.Throws<InvalidOperationException>(() => cart.AddOrUpdateProduct(productId, 21));
    }

    [Fact(DisplayName = "RemoveProduct should remove product from cart")]
    public void Given_CartWithProduct_When_RemovingProduct_Then_ProductIsRemoved()
    {
        // Arrange
        var cart = CartTestData.GenerateCartWithOneProduct();
        var productId = cart.Products[0].ProductId;

        // Act
        cart.RemoveProduct(productId);

        // Assert
        Assert.Empty(cart.Products);
    }

    [Fact(DisplayName = "GetItemTotal should apply correct discount")]
    public void Given_CartWithProduct_When_GettingItemTotal_Then_DiscountIsApplied()
    {
        // Arrange
        var cart = CartTestData.GenerateCartWithDiscountProduct();
        var product = cart.Products[0];
        decimal unitPrice = 100m;

        // Act
        var total = cart.GetItemTotal(product.ProductId, unitPrice);

        // Assert
        var expectedDiscount = Cart.GetDiscountPercentage(product.Quantity);
        var expectedTotal = product.Quantity * unitPrice * (1 - expectedDiscount);
        Assert.Equal(expectedTotal, total);
    }

    [Fact(DisplayName = "ValidateBusinessRules should throw for invalid quantities")]
    public void Given_CartWithInvalidProduct_When_Validating_Then_ShouldThrow()
    {
        // Arrange
        var cart = CartTestData.GenerateCartWithInvalidProduct();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => cart.ValidateBusinessRules());
    }
}