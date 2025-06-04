using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Unit tests for the SaleItem entity.
/// </summary>
public class SaleItemTests
{
    [Fact(DisplayName = "SaleItem properties should be set and retrieved correctly")]
    public void Given_ValidArguments_When_Constructed_Then_PropertiesAreSet()
    {
        // Arrange
        var item = SaleItemTestData.GenerateValidSaleItem();

        // Assert
        Assert.NotEqual(Guid.Empty, item.Id);
        Assert.NotEqual(Guid.Empty, item.SaleId);
        Assert.NotEqual(Guid.Empty, item.ProductId);
        Assert.Equal("Test Product", item.ProductName);
        Assert.Equal(5, item.Quantity);
        Assert.Equal(100m, item.UnitPrice);
        Assert.Equal(10m, item.Discount);
        Assert.Equal(490m, item.Total);
        Assert.False(item.Cancelled);
    }

    [Fact(DisplayName = "SaleItem Cancelled property should be settable")]
    public void Given_SaleItem_When_SetCancelled_Then_CancelledIsTrue()
    {
        // Arrange
        var item = SaleItemTestData.GenerateValidSaleItem();

        // Act
        item.Cancelled = true;

        // Assert
        Assert.True(item.Cancelled);
    }
}