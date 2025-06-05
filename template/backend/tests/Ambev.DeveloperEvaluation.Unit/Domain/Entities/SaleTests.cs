using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Unit tests for the Sale entity.
/// </summary>
public class SaleTests
{
    [Fact(DisplayName = "Sale should calculate discount amount and percentage correctly")]
    public void Given_Quantity_When_GetDiscountAmountAndPercentage_Then_CorrectValues()
    {
        // Arrange
        decimal unitPrice = 100m;

        // Act & Assert
        Assert.Equal(0m, Sale.GetDiscountAmount(unitPrice, 1));
        Assert.Equal(0.10m, Sale.GetDiscountPercentage(4));
        Assert.Equal(0.20m, Sale.GetDiscountPercentage(10));
        Assert.Equal(200m, Sale.GetDiscountAmount(unitPrice, 10));
    }

    [Fact(DisplayName = "Sale Validate should throw for invalid item quantity")]
    public void Given_SaleWithInvalidItem_When_Validate_Then_ShouldThrow()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithInvalidItem();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => sale.Validate());
    }

    [Fact(DisplayName = "Sale Validate should not throw for valid sale")]
    public void Given_ValidSale_When_Validate_Then_DoesNotThrow()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithOneItem();

        // Act & Assert
        sale.Validate();
    }

    [Fact(DisplayName = "Sale Cancelled should be true if all items are cancelled")]
    public void Given_SaleWithAllItemsCancelled_When_Validate_Then_CancelledIsTrue()
    {
        // Arrange
        var sale = SaleTestData.GenerateSaleWithAllItemsCancelled();

        // Act
        sale.Validate();

        // Assert
        Assert.True(sale.Cancelled);
    }
}